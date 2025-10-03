using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace ClientServicePortal.Core.Entities
{
  public class User : IdentityUser<string>
  {
    public User()
    {
      Id = Guid.NewGuid().ToString(); // Generate ID in constructor
    }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ServiceRequest> Requests { get; set; } = new List<ServiceRequest>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
  }
}
