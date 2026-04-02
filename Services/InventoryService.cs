using WebBusinessPromilexApp.Models;

public class InventoryService
{
    private readonly ApplicationDbContext _context;
    public InventoryService(ApplicationDbContext context) => _context = context;

    public async Task RegisterMovementAsync(InventoryLog log)
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

            _context.InventoryLogs.Add(log);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}