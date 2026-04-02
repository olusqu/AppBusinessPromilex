namespace WebBusinessPromilexApp.Models;

public partial class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
}
