using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;   

public class PromotionService
{
    private readonly ApplicationDbContext _context;
    public PromotionService(ApplicationDbContext context) => _context = context;

    public async Task<List<Promotion>> GetAllAsync() => await _context.Promotions.ToListAsync();
    public async Task<Promotion?> GetByIdAsync(int id) => await _context.Promotions.FindAsync(id);
    public async Task CreateAsync(Promotion promotion) { _context.Add(promotion); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Promotion promotion) { _context.Update(promotion); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var promo = await _context.Promotions.FindAsync(id); if (promo != null) { _context.Promotions.Remove(promo); await _context.SaveChangesAsync(); } }
    public bool Exists(int id) => _context.Promotions.Any(e => e.Id == id);
}
