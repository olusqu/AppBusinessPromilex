using Microsoft.AspNetCore.Mvc;
using WebBusinessPromilexApp.Services;

namespace WebBusinessPromilexApp.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View(_cartService.GetCart());
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product != null && product.IsAvailable && product.Stock > 0)
            {
                _cartService.AddToCart(product);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrease(int productId)
        {
            _cartService.DecreaseQuantity(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            _cartService.RemoveItem(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index");
        }
    }
}