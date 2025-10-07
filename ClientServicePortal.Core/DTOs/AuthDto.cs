using System.ComponentModel.DataAnnotations;

namespace ClientServicePortal.Core.DTOs
{
  public class LoginDto
  {
    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public required string Password { get; set; }
  }

  public class LoginResponseDto
  {
    public string Token { get; set; } = "";
    public string UserId { get; set; } = "";
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
  }

  public class RegisterDto
  {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
    public required string Password { get; set; }
    public required string Role { get; set; } = "view";
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }

  public class RegisterResponseDto
  {
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string message { get; set; } = "";
  }

  // TODO: refresh token
  // TODO: logout
  // TODO: restablish account
  // TODO: profile
}