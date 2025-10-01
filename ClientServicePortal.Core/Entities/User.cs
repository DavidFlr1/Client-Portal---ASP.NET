using System;
using System.Collections.Generic;

namespace ClientServicePortal.Core.Entities
{
  public class User
  {
    public int id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ServiceRequest> Requests { get; set; } = new List<ServiceRequest>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
  }
}