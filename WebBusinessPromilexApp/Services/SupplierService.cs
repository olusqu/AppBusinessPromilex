using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

public class SupplierService
{
    private readonly ApplicationDbContext _context;
    public SupplierService(ApplicationDbContext context) => _context = context;

    public async Task<List<Supplier>> GetAllAsync() => await _context.Suppliers.ToListAsync();
    public async Task<Supplier?> GetByIdAsync(int id) => await _context.Suppliers.FindAsync(id);
    public async Task CreateAsync(Supplier supplier) { _context.Add(supplier); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Supplier supplier) { _context.Update(supplier); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var sup = await _context.Suppliers.FindAsync(id); if (sup != null) { _context.Suppliers.Remove(sup); await _context.SaveChangesAsync(); } }
    public bool Exists(int id) => _context.Suppliers.Any(e => e.Id == id);
}
