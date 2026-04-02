using WebBusinessPromilexApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebBusinessPromilexApp.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _context;
        public AdminService(ApplicationDbContext context) => _context = context;

        public async Task<dynamic> GetDashboardStatsAsync()
        {
            return new
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalSuppliers = await _context.Suppliers.CountAsync(),
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalLogs = await _context.InventoryLogs.CountAsync()
            };
        }
    }
}
