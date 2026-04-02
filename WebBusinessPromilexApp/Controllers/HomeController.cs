using Microsoft.AspNetCore.Mvc;
using WebBusinessPromilexApp.Models;
using WebBusinessPromilexApp.Services;
namespace WebBusinessPromilexApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly MessageService _messageService;

        public HomeController(CategoryService categoryService, ProductService productService, MessageService messageService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.PremiumProducts = await _productService.GetPremiumAsync(4);
                return View();
            }
            catch
            {
                ViewBag.ErrorMessage = "Nie można połączyć się z bazą danych na stronie głównej.";
                return View("Error");
            }
        }

        public async Task<IActionResult> Shop(int? categoryId)
        {
            try
            {
                var products = categoryId.HasValue
                    ? await _productService.GetByCategoryAsync(categoryId)
                    : await _productService.GetAllAsync();

                if (categoryId.HasValue)
                {
                    var selectedCategory = await _categoryService.GetByIdAsync(categoryId.Value);
                    ViewBag.CurrentCategoryName = selectedCategory?.Name;
                }

                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(products);
            }
            catch
            {
                ViewBag.ErrorMessage = "Błąd połączenia z bazą w module Sklep.";
                return View("Error");
            }
        }

        public IActionResult Contact() => View(new Message());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("SenderName,Email,Phone,Content")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.SentDate = DateTime.Now;
                message.IsRead = false;
                await _messageService.CreateAsync(message);
                TempData["MessageSent"] = true;
                return RedirectToAction(nameof(Contact));
            }
            return View(message);
        }
    }
}