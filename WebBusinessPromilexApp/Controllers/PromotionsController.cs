using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Filters;
using WebBusinessPromilexApp.Models;

namespace WebBusinessPromilexApp.Controllers
{
    [ServiceFilter(typeof(AdminOnlyAttribute))]
    public class PromotionsController : Controller
    {
        private readonly PromotionService _service;
        public PromotionsController(PromotionService service) { _service = service; }

        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var promotion = await _service.GetByIdAsync(id.Value);
            if (promotion == null) return NotFound();
            return View(promotion);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DiscountPercentage,StartDate,EndDate")] Promotion promotion)
        {
            if (ModelState.IsValid) { await _service.CreateAsync(promotion); return RedirectToAction(nameof(Index)); }
            return View(promotion);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var promotion = await _service.GetByIdAsync(id.Value);
            if (promotion == null) return NotFound();
            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DiscountPercentage,StartDate,EndDate")] Promotion promotion)
        {
            if (id != promotion.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try { await _service.UpdateAsync(promotion); }
                catch (DbUpdateConcurrencyException) { if (!_service.Exists(promotion.Id)) return NotFound(); else throw; }
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var promotion = await _service.GetByIdAsync(id.Value);
            if (promotion == null) return NotFound();
            return View(promotion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) { await _service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    }
}
