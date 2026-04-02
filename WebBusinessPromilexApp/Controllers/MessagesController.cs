using Microsoft.AspNetCore.Mvc;
using WebBusinessPromilexApp.Filters;
namespace WebBusinessPromilexApp.Controllers
{
    [ServiceFilter(typeof(AdminOnlyAttribute))]
    public class MessagesController : Controller
    {
        private readonly MessageService _service;
        public MessagesController(MessageService service) { _service = service; }

        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var message = await _service.GetByIdAsync(id.Value);
            if (message == null) return NotFound();

            if (!message.IsRead)
            {
                message.IsRead = true;
                await _service.CreateAsync(message); 
            }
            return View(message);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var message = await _service.GetByIdAsync(id.Value);
            if (message == null) return NotFound();
            return View(message);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
