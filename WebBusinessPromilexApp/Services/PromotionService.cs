using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebBusinessPromilexApp.Services
{
    public class PromotionService
    {
        private readonly ApplicationDbContext _context;
        public PromotionService(ApplicationDbContext context) => _context = context;

        public async Task<List<Promotion>> GetAllAsync() =>
            await _context.Promotions.Include(p => p.Products).ToListAsync();

        public async Task<Promotion?> GetByIdAsync(int id) =>
            await _context.Promotions.Include(p => p.Products).FirstOrDefaultAsync(m => m.Id == id);

        public async Task CreateAsync(Promotion promotion)
        {
            _context.Add(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Promotion promotion)
        {
            _context.Update(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var promo = await _context.Promotions.FindAsync(id);
            if (promo != null)
            {
                _context.Promotions.Remove(promo);
                await _context.SaveChangesAsync();
            }
        }

        public bool Exists(int id) => _context.Promotions.Any(e => e.Id == id);

        public async Task<List<Product>> GetAllProductsAsync() =>
            await _context.Products.ToListAsync();
        public async Task AssignProductsToPromotionAsync(int promotionId, List<int> productIds)
        {
            var promotion = await _context.Promotions
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.Id == promotionId);

            if (promotion == null) return;

            promotion.Products.Clear();

            if (productIds != null && productIds.Any())
            {
                var selectedProducts = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var product in selectedProducts)
                {
                    promotion.Products.Add(product);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}