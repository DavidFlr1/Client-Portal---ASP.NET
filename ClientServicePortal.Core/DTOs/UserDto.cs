namespace ClientServicePortal.Core.DTOs
{
  public class GetUserProfileDto
  {
    public string UserId { get; set; } = "";
  }

  public class GetAllUsersDto
  {
    // Pagination
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    // Filtering
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
    
    // Sorting
    public string SortBy { get; set; } = "CreatedAt"; // FirstName, LastName, Email, CreatedAt
    public string SortOrder { get; set; } = "desc"; // asc, desc
  }

  public class UserProfileResponseDto
  {
    public string Id { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
  }

  public class PagedResponse<T>
  {
    public List<T> Data { get; set; } = new();
    public int TotalRecords { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
  }

}
