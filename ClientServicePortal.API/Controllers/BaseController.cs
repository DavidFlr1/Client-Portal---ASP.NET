using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClientServicePortal.Core.DTOs;

namespace ClientServicePortal.API.Controllers
{
  public class BaseController : ControllerBase
  {
    protected string? GetCurrentUserId()
    {
      return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    protected IActionResult SuccessResponse<T>(T data, string message = "Success")
    {
      return Ok(new BaseResponse<T>
      {
        Data = data,
        Message = message,
        Status = "success",
        RequestTime = DateTime.UtcNow,
        RequestBy = GetCurrentUserId()
      });
    }

    protected IActionResult ErrorResponse(string message, int statusCode = 400)
    {
      return StatusCode(statusCode, new BaseResponse
      {
        Data = null,
        Message = message,
        Status = "error",
        RequestTime = DateTime.UtcNow,
        RequestBy = GetCurrentUserId()
      });
    }

    protected IActionResult WarningResponse<T>(T data, string message)
    {
      return Ok(new BaseResponse<T>
      {
        Data = data,
        Message = message,
        Status = "warning",
        RequestTime = DateTime.UtcNow,
        RequestBy = GetCurrentUserId()
      });
    }
  }
}