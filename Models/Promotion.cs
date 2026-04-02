using System;
using System.Collections.Generic;

namespace WebBusinessPromilexApp.Models;

public partial class Promotion
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal DiscountPercentage { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
