using WebBusinessPromilexApp.Models;                               
using Microsoft.EntityFrameworkCore;        
public class CategoryService
{
    private readonly ApplicationDbContext _context;
    public CategoryService(ApplicationDbContext context) => _context = context;

    public async Task<List<Category>> GetAllAsync() => await _context.Categories.ToListAsync();
    public async Task<Category?> GetByIdAsync(int id) => await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
    public async Task CreateAsync(Category category) { _context.Add(category); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Category category) { _context.Update(category); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var cat = await _context.Categories.FindAsync(id); if (cat != null) { _context.Categories.Remove(cat); await _context.SaveChangesAsync(); } }
    public bool Exists(int id) => _context.Categories.Any(e => e.Id == id);
}