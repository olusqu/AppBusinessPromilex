using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebBusinessPromilexApp.Services
{
    public class InventoryLogService
    {
        private readonly ApplicationDbContext _context;
        public InventoryLogService(ApplicationDbContext context) => _context = context;

        public async Task<List<InventoryLog>> GetAllAsync() =>
            await _context.InventoryLogs.OrderByDescending(l => l.LogDate).ToListAsync();

        public async Task<InventoryLog?> GetByIdAsync(int id) =>
            await _context.InventoryLogs.FirstOrDefaultAsync(m => m.Id == id);

        public async Task CreateAsync(InventoryLog log)
        {
            log.LogDate = DateTime.Now;
            _context.InventoryLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InventoryLog log)
        {
            _context.Update(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _context.InventoryLogs.FindAsync(id);
            if (log != null) { _context.InventoryLogs.Remove(log); await _context.SaveChangesAsync(); }
        }

        public bool Exists(int id) => _context.InventoryLogs.Any(e => e.Id == id);
    }
}
