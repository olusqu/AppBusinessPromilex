using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context) => _context = context;

    public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();
    public async Task<Product?> GetByIdAsync(int id) => await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
    public async Task<List<Product>> GetAvailableAsync(int count) => await _context.Products.Where(p => p.IsAvailable).OrderByDescending(p => p.Price).Take(count).ToListAsync();
    public async Task CreateAsync(Product product) { _context.Add(product); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Product product) { _context.Update(product); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var prod = await _context.Products.FindAsync(id); if (prod != null) { _context.Products.Remove(prod); await _context.SaveChangesAsync(); } }
    public bool Exists(int id) => _context.Products.Any(e => e.Id == id);
}
