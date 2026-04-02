using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Filters;
using WebBusinessPromilexApp.Models;

namespace WebBusinessPromilexApp.Controllers
{
    [ServiceFilter(typeof(AdminOnlyAttribute))]
    public class EmployeesController : Controller
    {
        private readonly EmployeeService _service;
        public EmployeesController(EmployeeService service) { _service = service; }

        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _service.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();
            return View(employee);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Role,Username,PasswordHash")] Employee employee)
        {
            if (ModelState.IsValid) { await _service.CreateAsync(employee); return RedirectToAction(nameof(Index)); }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _service.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Role,Username,PasswordHash")] Employee employee)
        {
            if (id != employee.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try { await _service.UpdateAsync(employee); }
                catch (DbUpdateConcurrencyException) { if (!_service.Exists(employee.Id)) return NotFound(); else throw; }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _service.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) { await _service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    }
}
