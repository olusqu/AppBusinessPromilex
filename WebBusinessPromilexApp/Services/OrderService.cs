using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly ApplicationDbContext _context;
    public OrderService(ApplicationDbContext context) => _context = context;

    public async Task<List<Order>> GetAllAsync() => await _context.Orders.ToListAsync();
    public async Task<Order?> GetByIdAsync(int id) => await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
    public async Task CreateAsync(Order order) { _context.Add(order); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Order order) { _context.Update(order); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var ord = await _context.Orders.FindAsync(id); if (ord != null) { _context.Orders.Remove(ord); await _context.SaveChangesAsync(); } }
    public bool Exists(int id) => _context.Orders.Any(e => e.Id == id);
}
