using System;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;

namespace TutorSystem.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(string fullName, string email, string password, string phoneNumber, string university, string role);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<string?> LoginUserAsync(string email, string password); // ✅ Trả về trang chuyển hướng thay vì bool
        Task LogoutUserAsync();
        Task<User> GetUserProfileAsync(Guid userId);
        Task<bool> UpdateUserProfileAsync(User user);
    }
}
