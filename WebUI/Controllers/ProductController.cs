using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using System.Text;
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
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string email)
        {
            var cartItems = CartService.GetCartItems();
            // Creează lista de produse și pregătește obiectul JSON
            var orderItems = cartItems.Select(item => new
            {
                clientEmail = email,
                productCode = item.Product.Id,
                quantity = item.Quantity
            }).ToList();

            // Trimite cererea JSON către API-ul extern
            using (var client = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(orderItems);
                Console.WriteLine(jsonContent);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7195/ClientOrder/placeOrder", content);

                if (response.IsSuccessStatusCode)
                {
                    // Comanda a fost procesată cu succes
                    return RedirectToAction("OrderSuccess");
                }
                else
                {
                    // Există o eroare
                    ModelState.AddModelError(string.Empty, "A apărut o eroare la plasarea comenzii.");
                    return View("Cart", CartService.GetCartItems()); // Sau redirecționează la pagina coșului
                }
            }
        }
        public IActionResult OrderSuccess()
        {
            CartService.ClearCart();
            return View();
        }

    }
}

