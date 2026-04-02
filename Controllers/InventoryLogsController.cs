using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Models;
using WebBusinessPromilexApp.Services;

namespace WebBusinessPromilexApp.Controllers
{
    public class InventoryLogsController : Controller
    {
        private readonly InventoryLogService _logService;
        private readonly ApplicationDbContext _context;

        public InventoryLogsController(InventoryLogService logService, ApplicationDbContext context)
        {
            _logService = logService;
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _logService.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var log = await _logService.GetByIdAsync(id.Value);
            if (log == null) return NotFound();
            return View(log);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,EmployeeId,QuantityChange,TransactionType,Note")] InventoryLog inventoryLog)
        {
            if (ModelState.IsValid)
            {
                await _logService.RegisterMovementAsync(inventoryLog);
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(inventoryLog);
            return View(inventoryLog);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var log = await _logService.GetByIdAsync(id.Value);
            if (log == null) return NotFound();
            PopulateDropdowns(log);
            return View(log);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,EmployeeId,QuantityChange,TransactionType,LogDate,Note")] InventoryLog inventoryLog)
        {
            if (id != inventoryLog.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _logService.UpdateAsync(inventoryLog);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_logService.Exists(inventoryLog.Id)) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(inventoryLog);
            return View(inventoryLog);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var log = await _logService.GetByIdAsync(id.Value);
            if (log == null) return NotFound();
            return View(log);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _logService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        private void PopulateDropdowns(InventoryLog? log = null)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", log?.ProductId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Username", log?.EmployeeId);
        }
    }
}