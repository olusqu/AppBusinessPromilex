using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

public class MessageService
{
    private readonly ApplicationDbContext _context;
    public MessageService(ApplicationDbContext context) => _context = context;

    public async Task<List<Message>> GetAllAsync() => await _context.Messages.OrderByDescending(m => m.SentDate).ToListAsync();
    public async Task<Message?> GetByIdAsync(int id) => await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
    public async Task CreateAsync(Message message) { _context.Messages.Add(message); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var msg = await _context.Messages.FindAsync(id); if (msg != null) { _context.Messages.Remove(msg); await _context.SaveChangesAsync(); } }
}
