using WebUI.Models;

namespace WebUI.Services
{
    public static class CartService
    {
        private static readonly List<CartItemViewModel> _cartItems = new List<CartItemViewModel>();
        private static string trueEmail; 
        public static List<CartItemViewModel> GetCartItems() => _cartItems;

        public static void SetEmail(string mail)
        {
            trueEmail = mail;
        }

        public static string GetEmail()
        {
            return trueEmail;
        }
        public static void AddToCart(ProductViewModel product)
        {
            var existingItem = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                _cartItems.Add(new CartItemViewModel { Product = product, Quantity = 1 });
            }
        }

        public static void RemoveFromCart(string productId)
        {
            var item = _cartItems.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        public static decimal? GetTotal()
        {
            return _cartItems.Sum(item => item.Product.Price * item.Quantity);
        }
        public static void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}
