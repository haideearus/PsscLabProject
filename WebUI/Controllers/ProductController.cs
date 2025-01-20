using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController( IProductRepository productRepository)
        {
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
        public IActionResult AddToCart(string productId, decimal productPrice, string productName)
        {
            


            return RedirectToAction("Cart");
        }

        // Afișează coșul de cumpărături
        public IActionResult Cart(string productId, decimal productPrice, string productName)
        {
            var cartItems = CartService.GetCartItems();
            var total = CartService.GetTotal();
            var product = new ProductViewModel { Id = productId, Name = productName, Price = productPrice };
            CartService.AddToCart(product);

            ViewBag.Total = total+productPrice;
            return View(cartItems);
        }
        public IActionResult CartRemove(string productId, decimal productPrice)
        {
            var cartItems = CartService.GetCartItems();
            var total = CartService.GetTotal();
            CartService.RemoveFromCart(productId);

            ViewBag.Total = total + productPrice;
            return View(cartItems);
        }
    }
}

