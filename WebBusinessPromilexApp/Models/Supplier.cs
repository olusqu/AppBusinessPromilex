namespace WebBusinessPromilexApp.Models;

public partial class Supplier
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
}
