using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Models;
using WebBusinessPromilexApp.Services;
using WebBusinessPromilexApp.Filters;

namespace WebBusinessPromilexApp.Controllers
{
    [ServiceFilter(typeof(AdminOnlyAttribute))]
    public class InventoryLogsController : Controller
    {
        private readonly InventoryLogService _logService;

        public InventoryLogsController(InventoryLogService logService)
        {
            _logService = logService;
        }

        public async Task<IActionResult> Index() => View(await _logService.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var log = await _logService.GetByIdAsync(id.Value);
            if (log == null) return NotFound();
            return View(log);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuantityChange,TransactionType,Note")] InventoryLog inventoryLog)
        {
            if (ModelState.IsValid)
            {
                await _logService.CreateAsync(inventoryLog);
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryLog);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var log = await _logService.GetByIdAsync(id.Value);
            if (log == null) return NotFound();
            return View(log);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuantityChange,TransactionType,LogDate,Note")] InventoryLog inventoryLog)
        {
            if (id != inventoryLog.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try { await _logService.UpdateAsync(inventoryLog); }
                catch (DbUpdateConcurrencyException) { if (!_logService.Exists(inventoryLog.Id)) return NotFound(); else throw; }
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> DeleteConfirmed(int id) { await _logService.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    }
}
