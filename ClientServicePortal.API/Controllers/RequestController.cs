using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientServicePortal.Core.DTOs;
using ClientServicePortal.Core.Entities;
using ClientServicePortal.Infrastructure.Data;

namespace ClientServicePortal.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class RequestController : BaseController
  {
    private readonly AppDbContext _context;

    public RequestController(AppDbContext context)
    {
      _context = context;
    }

    // Showcase: EF Core
    [HttpPost("createRequest")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRequest([FromBody] PostRequestDto body)
    {
      try
      {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Create ServiceRequest entity
        var serviceRequest = new ServiceRequest
        {
          Title = body.Title,
          Description = body.Description,
          Status = body.Status,
          UserId = body.UserId,
          CreatedAt = DateTime.UtcNow
        };

        // Add to context and save
        _context.ServiceRequests.Add(serviceRequest);
        await _context.SaveChangesAsync();

        return SuccessResponse(new
        {
          Id = serviceRequest.Id,
          Title = serviceRequest.Title,
          Status = serviceRequest.Status,
          CreatedAt = serviceRequest.CreatedAt
        }, "Request created successfully");
      }
      catch (Exception ex)
      {
        return ErrorResponse($"Internal server error: {ex.Message}", 500);
      }
    }




    // Showcase: Raw SQL
    [HttpPost("createRequest-sql")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRequestRawSql([FromBody] PostRequestDto body)
    {
      try
      {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sql = @"
          INSERT INTO ServiceRequests (UserId, Title, Description, Status, CreatedAt)
          VALUES ({0}, {1}, {2}, {3}, {4})";

        var result = await _context.Database.ExecuteSqlRawAsync(sql,
          body.UserId, body.Title, body.Description, body.Status, DateTime.UtcNow);

        // Get the last inserted record
        var lastRequest = await _context.ServiceRequests
          .Where(r => r.UserId == body.UserId && r.Title == body.Title)
          .OrderByDescending(r => r.Id)
          .FirstOrDefaultAsync();

        return SuccessResponse(result, "Request created successfully (Raw SQL)");
      }
      catch (Exception ex)
      {
        return ErrorResponse($"Internal server error: {ex.Message}", 500);
      }
    }

    // Showcase: Stored Procedure
    [HttpPost("createRequest-sp")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRequestStoredProcedure([FromBody] PostRequestDto body)
    {
      try
      {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _context.Database.ExecuteSqlRawAsync(
          "EXEC sp_CreateServiceRequest @UserId = {0}, @Title = {1}, @Description = {2}",
          body.UserId, body.Title, body.Description);

        return SuccessResponse(new
        {
          RowsAffected = result,
          Message = "Request created via stored procedure"
        }, "Request created successfully (Stored Procedure)");
      }
      catch (Exception ex)
      {
        return ErrorResponse($"Internal server error: {ex.Message}", 500);
      }
    }
  }
  
}
