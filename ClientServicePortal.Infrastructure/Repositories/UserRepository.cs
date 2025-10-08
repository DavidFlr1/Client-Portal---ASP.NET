using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClientServicePortal.Core.Entities;
using ClientServicePortal.Core.Interfaces;
using ClientServicePortal.Core.DTOs;

namespace ClientServicePortal.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
      _userManager = userManager;
    }

    public async Task<User?> FindUserByIdAsync(string userId)
    {
      return await _userManager.FindByIdAsync(userId);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
      return await _userManager.Users.ToListAsync();
    }

    public async Task<PagedResponse<UserProfileResponseDto>> GetFilteredUsersAsync(GetAllUsersDto query)
    {
      var usersQuery = _userManager.Users.AsQueryable();

      // Apply filters
      if (!string.IsNullOrEmpty(query.FirstName))
        usersQuery = usersQuery.Where(u => u.FirstName.Contains(query.FirstName));
      
      if (!string.IsNullOrEmpty(query.LastName))
        usersQuery = usersQuery.Where(u => u.LastName.Contains(query.LastName));
      
      if (!string.IsNullOrEmpty(query.Email))
        usersQuery = usersQuery.Where(u => u.Email!.Contains(query.Email));
      
      if (!string.IsNullOrEmpty(query.Role))
        usersQuery = usersQuery.Where(u => u.Role == query.Role);
      
      if (query.IsActive.HasValue)
        usersQuery = usersQuery.Where(u => u.IsActive == query.IsActive.Value);

      // Apply sorting
      usersQuery = query.SortBy.ToLower() switch
      {
        "firstname" => query.SortOrder == "asc" ? usersQuery.OrderBy(u => u.FirstName) : usersQuery.OrderByDescending(u => u.FirstName),
        "lastname" => query.SortOrder == "asc" ? usersQuery.OrderBy(u => u.LastName) : usersQuery.OrderByDescending(u => u.LastName),
        "email" => query.SortOrder == "asc" ? usersQuery.OrderBy(u => u.Email) : usersQuery.OrderByDescending(u => u.Email),
        _ => query.SortOrder == "asc" ? usersQuery.OrderBy(u => u.CreatedAt) : usersQuery.OrderByDescending(u => u.CreatedAt)
      };

      // Get total count before pagination
      var totalRecords = await usersQuery.CountAsync();

      // Apply pagination
      var users = await usersQuery
        .Skip((query.Page - 1) * query.PageSize)
        .Take(query.PageSize)
        .Select(u => new UserProfileResponseDto
        {
          Id = u.Id,
          FirstName = u.FirstName,
          LastName = u.LastName,
          FullName = u.FullName,
          Email = u.Email!,
          Role = u.Role,
          IsActive = u.IsActive,
          CreatedAt = u.CreatedAt
        })
        .ToListAsync();

      return new PagedResponse<UserProfileResponseDto>
      {
        Data = users,
        TotalRecords = totalRecords,
        Page = query.Page,
        PageSize = query.PageSize,
        TotalPages = (int)Math.Ceiling((double)totalRecords / query.PageSize)
      };
    }
  }
}
