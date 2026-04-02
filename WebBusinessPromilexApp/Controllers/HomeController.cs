using Microsoft.AspNetCore.Mvc;
using WebBusinessPromilexApp.Models;

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
                ViewBag.PremiumProducts = await _productService.GetAvailableAsync(4);
                return View();
            }
            catch
            {
                ViewBag.ErrorMessage = "Nie można połączyć się z bazą danych.";
                return View("Error");
            }
        }

        public async Task<IActionResult> Shop()
        {
            try
            {
                var products = await _productService.GetAllAsync();
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
                TempData["SuccessMessage"] = "Wiadomość została wysłana!";
                return RedirectToAction(nameof(Contact));
            }
            return View(message);
        }
    }
}
