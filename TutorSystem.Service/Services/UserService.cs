using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;
using TutorSystem.Repository.Interfaces;
using TutorSystem.Service.Interfaces;

namespace TutorSystem.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITutorService _tutorService;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ITutorService tutorService)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _tutorService = tutorService;
        }

        public async Task<User> RegisterUserAsync(string fullName, string email, string password, string phoneNumber, string university, string role)
        {
            if (await _userRepository.UserExistsAsync(email))
                throw new Exception("Email đã tồn tại!");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FullName = fullName,
                Email = email,
                PasswordHash = HashPassword(password),
                PhoneNumber = phoneNumber,
                University = university,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUserAsync(user);
            return user;
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !VerifyHashedPassword(user.PasswordHash, password))
                throw new Exception("Email hoặc mật khẩu không đúng!");

            return user;
        }

        private bool VerifyHashedPassword(string hashedPassword, string inputPassword)
        {
            var inputHash = HashPassword(inputPassword);
            return hashedPassword == inputHash;
        }

        public async Task<string?> LoginUserAsync(string email, string password)
        {
            try
            {
                var user = await AuthenticateUserAsync(email, password);
                if (user != null && _httpContextAccessor.HttpContext != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    };

                    await _httpContextAccessor.HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    if (user.Role == "Tutor")
                    {
                        var tutor = await _tutorService.GetTutorByUserIdAsync(user.UserId);
                        if (tutor != null && !(tutor.IsApproved ?? false))
                        {
                            return "/Account/TutorVerification"; // 🚀 Nếu chưa duyệt, chuyển hướng xác minh
                        }
                    }

                    return "/Index"; // ✅ Nếu không phải tutor hoặc đã xác minh, về trang chính
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đăng nhập: {ex.Message}");
                return null;
            }
        }
        public async Task LogoutUserAsync()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<User> GetUserProfileAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }
}
