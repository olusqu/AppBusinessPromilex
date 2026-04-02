using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

public class EmployeeService
{
    private readonly ApplicationDbContext _context;
    public EmployeeService(ApplicationDbContext context) => _context = context;

    public async Task<List<Employee>> GetAllAsync() => await _context.Employees.ToListAsync();
    public async Task<Employee?> GetByIdAsync(int id) => await _context.Employees.FindAsync(id);
    public async Task CreateAsync(Employee employee) { _context.Add(employee); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Employee employee) { _context.Update(employee); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var emp = await _context.Employees.FindAsync(id); if (emp != null) { _context.Employees.Remove(emp); await _context.SaveChangesAsync(); } }
    public bool Exists(int id) => _context.Employees.Any(e => e.Id == id);
}
