using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

public class CustomerService
{
    private readonly ApplicationDbContext _context;
    public CustomerService(ApplicationDbContext context) => _context = context;

    public async Task<List<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();
    public async Task<Customer?> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);
    public async Task CreateAsync(Customer customer) { _context.Add(customer); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Customer customer) { _context.Update(customer); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var cust = await _context.Customers.FindAsync(id); if (cust != null) { _context.Customers.Remove(cust); await _context.SaveChangesAsync(); } }
    public bool Exists(int id) => _context.Customers.Any(e => e.Id == id);
}
