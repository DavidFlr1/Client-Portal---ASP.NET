
namespace ClientServicePortal.Core.DTOs
{
  public class LoginDto
  {
    public required string Username { get; set; }
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

  // Restablish account
}