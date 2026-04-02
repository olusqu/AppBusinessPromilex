using System;

namespace WebBusinessPromilexApp.Models;

public class InventoryLog
{
    public int Id { get; set; }
    public int QuantityChange { get; set; }
    public string TransactionType { get; set; } = "Dostawa";
    public DateTime LogDate { get; set; } = DateTime.Now;
    public string? Note { get; set; }
}
