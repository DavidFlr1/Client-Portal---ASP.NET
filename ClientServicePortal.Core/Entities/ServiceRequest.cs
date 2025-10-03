namespace ClientServicePortal.Core.Entities
{
  public class ServiceRequest
  {
    public int Id { get; set; }
    public string UserId { get; set; } = "";

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    
  }
}