using ClientServicePortal.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClientServicePortal.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> FindUserByUsernameAsync(string username);
        Task<User?> FindUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<bool> UserExistsAsync(string email);
    }
}