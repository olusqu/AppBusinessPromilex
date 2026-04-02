using WebBusinessPromilexApp.Models;
using WebBusinessPromilexApp.Helpers;

namespace WebBusinessPromilexApp.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "PromilexCart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession? Session => _httpContextAccessor.HttpContext?.Session;

        public List<CartItem> GetCart()
        {
            return Session?.Get<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
        }

        public void AddToCart(Product product)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == product.Id);

            if (existingItem != null)
            {
                if (existingItem.Quantity < product.Stock)
                {
                    existingItem.Quantity++;
                }
            }
            else
            {
                decimal finalPrice = product.Price;
                var activePromo = product.Promotions?.FirstOrDefault(p => p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now);

                if (activePromo != null)
                {
                    finalPrice = product.Price * (1 - (activePromo.DiscountPercentage / 100m));
                }

                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = finalPrice,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }

            Session?.Set(CartSessionKey, cart);
        }

        public void DecreaseQuantity(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    cart.Remove(item);
                }
                Session?.Set(CartSessionKey, cart);
            }
        }

        public void RemoveItem(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                Session?.Set(CartSessionKey, cart);
            }
        }

        public void ClearCart()
        {
            Session?.Remove(CartSessionKey);
        }
    }
}