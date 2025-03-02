using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;
using TutorSystem.Repository.Interfaces;

namespace TutorSystem.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TutorSystemContext _context;

        public UserRepository(TutorSystemContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (user.Role == "Tutor")
            {
                var tutor = new Tutor
                {
                    TutorId = Guid.NewGuid(),
                    UserId = user.UserId,
                    Bio = "Thông tin chưa cập nhật", // 🔥 Giá trị mặc định
                    HourlyRate = 100000, // 🔥 Đặt mức giá mặc định
                    Rating = 0,
                    ReviewsCount = 0,
                    AvailableHours = "Chưa cập nhật",
                    IsApproved = false, // 🚀 Tutor chưa xác minh
                    ApprovedBy = null,
                    ApprovedAt = null
                };

                _context.Tutors.Add(tutor);
                await _context.SaveChangesAsync();
            }
            else if (user.Role == "Student")
            {
                var student = new Student
                {
                    StudentId = Guid.NewGuid(),
                    UserId = user.UserId
                };
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.University = user.University;
                existingUser.UpdatedAt = DateTime.UtcNow;

                // Không cập nhật PasswordHash nếu không có thay đổi
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
