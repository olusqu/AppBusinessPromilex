using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;


namespace WebBusinessPromilexApp.Services
{
    public class InventoryLogService
    {
        private readonly ApplicationDbContext _context;
        public InventoryLogService(ApplicationDbContext context) => _context = context;

        public async Task<List<InventoryLog>> GetAllAsync() =>
            await _context.InventoryLogs.Include(i => i.Employee).Include(i => i.Product).OrderByDescending(l => l.LogDate).ToListAsync();

        public async Task<InventoryLog?> GetByIdAsync(int id) =>
            await _context.InventoryLogs.Include(i => i.Employee).Include(i => i.Product).FirstOrDefaultAsync(m => m.Id == id);

        public async Task DeleteAsync(int id)
        {
            var log = await _context.InventoryLogs.FindAsync(id);
            if (log != null) { _context.InventoryLogs.Remove(log); await _context.SaveChangesAsync(); }
        }

        public bool Exists(int id) => _context.InventoryLogs.Any(e => e.Id == id);

        public async Task UpdateAsync(InventoryLog log)
        {
            _context.Update(log);
            await _context.SaveChangesAsync();
        }
        public async Task RegisterMovementAsync(InventoryLog log)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var product = await _context.Products.FindAsync(log.ProductId);
                    if (product != null)
                    {
                        product.Stock += log.QuantityChange;
                        _context.Update(product);
                    }

                    log.LogDate = DateTime.Now;
                    _context.InventoryLogs.Add(log);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}