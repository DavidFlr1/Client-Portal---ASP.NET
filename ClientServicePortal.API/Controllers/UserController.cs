using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ClientServicePortal.Core.DTOs;
using ClientServicePortal.Core.Entities;
using ClientServicePortal.Core.Interfaces;
using ClientServicePortal.Infrastructure.Repositories;

namespace ClientServicePortal.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize] // Requires valid JWT token
  public class UserController : BaseController
  {
    // private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userManager;
    public UserController(IUserRepository userRepository)
    {
      _userManager = userRepository;
      // _userRepository = userRepository;
    }

    [HttpGet("userProfile")]
    public async Task<IActionResult> GetProfile([FromQuery] GetUserProfileDto query)
    {
      try
      {
        
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(query.UserId))
          return ErrorResponse("UserId is required");

        var user = await _userManager.FindUserByIdAsync(query.UserId);
        
        if (user == null)
          return ErrorResponse("User not found", 404);

        return SuccessResponse(user, "Profile retrieved successfully");
      }
      catch (Exception ex)
      {
        return ErrorResponse($"Internal server error: {ex.Message}", 500);
      }
    }

    [HttpGet("allUsersProfiles")]
    public async Task<IActionResult> GetAllProfiles([FromQuery] GetAllUsersDto query)
    {
      try
      {
        if (!ModelState.IsValid)
          return ErrorResponse("Invalid query parameters");

        var response = await _userManager.GetFilteredUsersAsync(query);
        return SuccessResponse(response, "Users retrieved successfully");
      }
      catch (Exception ex)
      {
        return ErrorResponse($"Internal server error: {ex.Message}", 500);
      }
    }





    [HttpGet("admin-only")]
    [Authorize(Roles = "Admin")] // Only Admin role can access
    public IActionResult AdminOnly()
    {
      return Ok(new { message = "Admin access granted" });
    }

    [HttpGet("public")]
    [AllowAnonymous] // Override [Authorize] for this specific endpoint
    public IActionResult PublicEndpoint()
    {
      return Ok(new { message = "This is public" });
    }
  }
}
