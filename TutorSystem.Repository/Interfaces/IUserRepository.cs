using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;

namespace TutorSystem.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task<bool> UserExistsAsync(string email);
        Task UpdateUserAsync(User user);  // Cập nhật thông tin người dùng
    }
}
