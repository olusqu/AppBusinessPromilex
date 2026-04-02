using System;

namespace WebBusinessPromilexApp.Models;

public class InventoryLog
{
    public int Id { get; set; }

    // Relacja z produktem
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }

    // Kto dokonał zmiany (Relacja z pracownikiem)
    public int? EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }

    public int QuantityChange { get; set; } // np. +50 (dostawa) lub -5 (sprzedaż)
    public string TransactionType { get; set; } = "Dostawa"; // Dostawa, Sprzedaż, Korekta, Zwrot
    public DateTime LogDate { get; set; } = DateTime.Now;
    public string? Note { get; set; } // np. "Dostawa od Supplier X"
}