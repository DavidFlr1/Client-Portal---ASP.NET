using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using ClientServicePortal.Core.Entities;
using ClientServicePortal.Core.DTOs;
using ClientServicePortal.Core.Interfaces;

namespace ClientServicePortal.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthRepository authRepository, IConfiguration configuration)
    {
      _authRepository = authRepository;
      _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new { 
          error = "Validation failed", 
          details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) 
        });
      }

      try
      {
        var user = await _authRepository.FindUserByUsernameAsync(model.Username);
        if (user == null)
          return Unauthorized("Invalid credentials");

        var isValidPassword = await _authRepository.CheckPasswordAsync(user, model.Password);
        if (!isValidPassword)
          return Unauthorized("Invalid credentials");

        var token = GenerateJwtToken(user);
        
        return Ok(new LoginResponseDto
        {
          Token = token,
          UserId = user.Id,
          Email = user.Email!,
          FullName = user.FullName,
          ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { error = "Internal server error", message = ex.Message });
      }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new { 
          error = "Validation failed", 
          details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) 
        });
      }

      try
      {
        // Input sanitization
        model.FirstName = model.FirstName.Trim();
        model.LastName = model.LastName.Trim();
        model.UserName = model.UserName.Trim().ToLower();
        model.Email = model.Email.Trim().ToLower();

        var existingUser = await _authRepository.FindUserByEmailAsync(model.Email);
        if (existingUser != null)
          return BadRequest(new { error = "User with this email already exists" });

        var user = new User
        {
          UserName = model.UserName,
          Email = model.Email,
          FirstName = model.FirstName,
          LastName = model.LastName,
          FullName = $"{model.FirstName} {model.LastName}",
          Role = model.Role,
          PhoneNumber = model.PhoneNumber,
          IsActive = model.IsActive,
          CreatedAt = model.CreatedAt
        };

        var result = await _authRepository.CreateUserAsync(user, model.Password);

        if (!result.Succeeded)
        {
          return BadRequest(new { 
            error = "Registration failed", 
            details = result.Errors.Select(e => e.Description) 
          });
        }

        return Ok(new RegisterResponseDto
        {
          UserId = user.Id,
          UserName = user.UserName!,
          message = "User created successfully"
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { error = "Internal server error", message = ex.Message });
      }
    }

    private string GenerateJwtToken(User user)
    {
      var jwtSettings = _configuration.GetSection("JwtSettings");
      var secretKey = jwtSettings["SecretKey"]!;
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("FullName", user.FullName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var token = new JwtSecurityToken(
        issuer: jwtSettings["Issuer"],
        audience: jwtSettings["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(60),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
