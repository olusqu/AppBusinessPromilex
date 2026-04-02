using System.Collections.Generic;

namespace WebBusinessPromilexApp.Models;

public partial class Category
{
    public Category()
    {
        Products = new HashSet<Product>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Pole na obrazek kategorii (URL)
    public string? ImageUrl { get; set; }

    // Relacja: Jedna kategoria ma wiele produktów
    public virtual ICollection<Product> Products { get; set; }
}