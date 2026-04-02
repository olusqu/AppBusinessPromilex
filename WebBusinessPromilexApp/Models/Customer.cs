using System.Collections.Generic;

namespace WebBusinessPromilexApp.Models;

public partial class Customer
{
    public Customer()
    {
        Orders = new HashSet<Order>();
    }

    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }

    // Relacja: Jeden klient może mieć wiele zamówień
    public virtual ICollection<Order> Orders { get; set; }
}