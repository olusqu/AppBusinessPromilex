using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Filters;
using WebBusinessPromilexApp.Models;

namespace WebBusinessPromilexApp.Controllers
{
    [ServiceFilter(typeof(AdminOnlyAttribute))]
    public class OrdersController : Controller
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service) { _service = service; }

        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var order = await _service.GetByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,TotalAmount,Status")] Order order)
        {
            if (ModelState.IsValid) { await _service.CreateAsync(order); return RedirectToAction(nameof(Index)); }
            return View(order);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _service.GetByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,TotalAmount,Status")] Order order)
        {
            if (id != order.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try { await _service.UpdateAsync(order); }
                catch (DbUpdateConcurrencyException) { if (!_service.Exists(order.Id)) return NotFound(); else throw; }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var order = await _service.GetByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) { await _service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    }
}
