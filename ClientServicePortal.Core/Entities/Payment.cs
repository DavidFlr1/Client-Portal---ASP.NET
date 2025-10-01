using System;
using System.Collections.Generic;

namespace ClientServicePortal.Core.Entities
{
  public class Payment
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? RequestId { get; set; }

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = "Pending";
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public ServiceRequest? Request { get; set; }
  }
}