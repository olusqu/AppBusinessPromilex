using System.Collections.Generic;

namespace WebBusinessPromilexApp.Models;

public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}