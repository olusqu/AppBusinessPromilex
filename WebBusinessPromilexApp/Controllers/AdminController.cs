using Microsoft.AspNetCore.Mvc;
using WebBusinessPromilexApp.Models;
using WebBusinessPromilexApp.Services;
using WebBusinessPromilexApp.Filters;

namespace WebBusinessPromilexApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AuthenticationService _authService;
        private readonly AdminService _adminService;

        public AdminController(AuthenticationService authService, AdminService adminService)
        {
            _authService = authService;
            _adminService = adminService;
        }

        [AdminOnly]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var stats = await _adminService.GetDashboardStatsAsync();

            ViewBag.TotalProducts = stats.TotalProducts;
            ViewBag.TotalCategories = stats.TotalCategories;
            ViewBag.TotalCustomers = stats.TotalCustomers;
            ViewBag.TotalSuppliers = stats.TotalSuppliers;
            ViewBag.TotalEmployees = stats.TotalEmployees;
            ViewBag.TotalLogs = stats.TotalLogs;

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("IsAdminLoggedIn") == "true")
                return RedirectToAction("Index", "Admin");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _authService.LoginAsync(model.Username, model.Password))
                    return RedirectToAction("Index", "Admin");
                ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło.");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Login", "Admin");
        }
    }
}
