using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientServicePortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires valid JWT token
    public class UserController : ControllerBase
    {
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
            return Ok(new { userId, email, message = "Protected endpoint accessed" });
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