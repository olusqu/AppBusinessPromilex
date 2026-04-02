using System;

namespace WebBusinessPromilexApp.Models;

public partial class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
}
