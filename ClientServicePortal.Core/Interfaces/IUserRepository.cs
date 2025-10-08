using ClientServicePortal.Core.Entities;
using ClientServicePortal.Core.DTOs;

namespace ClientServicePortal.Core.Interfaces
{
  public interface IUserRepository
  {
    Task<User?> FindUserByIdAsync(string userId);
    Task<List<User>> GetAllUsersAsync();
    Task<PagedResponse<UserProfileResponseDto>> GetFilteredUsersAsync(GetAllUsersDto query);
  }
}
