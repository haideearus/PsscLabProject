using Microsoft.AspNetCore.Mvc;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly CartService _cartService;
        private readonly IProductRepository _productRepository;
        public ProductController(CartService cartService, IProductRepository productRepository)
        {
            _cartService = cartService;
            _productRepository = productRepository;
        }

        public async Task<List<ProductViewModel>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            var modelProd = products.Select(dto => new ProductViewModel{
                Id= dto.ProductCode,
                Name=dto.ProductName,
                Price=dto.ProductPrice,
            }).ToList();
            return modelProd;
        }
        // Afișează lista de produse
        public async Task<IActionResult> Index()
        {
            
            // Exemplu de produse
            var products = await GetAllProducts();

            return View(products);
        }

        // Adaugă produs în coș
        [HttpPost]
        public IActionResult AddToCart(string productId, decimal productPrice)
        {
            var product = new ProductViewModel { Id = productId, Name = "Produs " + productId, Price = productPrice };
            _cartService.AddToCart(product);

            return RedirectToAction("Cart");
        }

        // Afișează coșul de cumpărături
        public IActionResult Cart()
        {
            var cartItems = _cartService.GetCartItems();
            var total = _cartService.GetTotal();

            ViewBag.Total = total;
            return View(cartItems);
        }
    }
}

