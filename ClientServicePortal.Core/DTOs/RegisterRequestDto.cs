namespace ClientServicePortal.Core.DTOs
{
  public class PostRequestDto
  {
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Pending";
    public string UserId { get; set; } = "";
  }
}
