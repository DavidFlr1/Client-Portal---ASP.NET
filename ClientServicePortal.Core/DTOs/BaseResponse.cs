namespace ClientServicePortal.Core.DTOs
{
  public class BaseResponse<T>
  {
    public T? Data { get; set; }
    public string Message { get; set; } = "";
    public string Status { get; set; } = "success"; // success, warning, error
    public DateTime RequestTime { get; set; } = DateTime.UtcNow;
    public string? RequestBy { get; set; }
  }

  public class BaseResponse : BaseResponse<object>
  {
  }
}