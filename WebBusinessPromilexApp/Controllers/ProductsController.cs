using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Filters;
using WebBusinessPromilexApp.Models;

namespace WebBusinessPromilexApp.Controllers
{
    [ServiceFilter(typeof(AdminOnlyAttribute))]
    public class ProductsController : Controller
    {
        private readonly ProductService _service;
        public ProductsController(ProductService service) { _service = service; }

        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var product = await _service.GetByIdAsync(id.Value);
            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Stock,IsAvailable,ImageUrl")] Product product)
        {
            if (ModelState.IsValid) { await _service.CreateAsync(product); return RedirectToAction(nameof(Index)); }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var product = await _service.GetByIdAsync(id.Value);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Stock,IsAvailable,ImageUrl")] Product product)
        {
            if (id != product.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try { await _service.UpdateAsync(product); }
                catch (DbUpdateConcurrencyException) { if (!_service.Exists(product.Id)) return NotFound(); else throw; }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _service.GetByIdAsync(id.Value);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) { await _service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    }
}
