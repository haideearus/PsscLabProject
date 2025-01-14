using System.ComponentModel.DataAnnotations;
using PsscFinalProject.Domain.Models;

namespace PsscFinalProject.Api.Models
{
    public class InputOrder
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string ClientEmail { get; set; }

        [Required]
        public List<InputProduct> ProductList { get; set; }

        public InputOrder(string clientEmail, List<InputProduct> productList)
        {
            if (string.IsNullOrEmpty(clientEmail))
                throw new ArgumentNullException(nameof(clientEmail), "ClientEmail cannot be null or empty");

            ClientEmail = clientEmail;
            ProductList = productList ?? throw new ArgumentNullException(nameof(productList), "ProductList cannot be null");
        }
    }

    public class InputProduct
    {
        [Required]
        public int ProductId { get; set; } 

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        public int Quantity { get; set; }

        public InputProduct(int productId, string name, decimal price, int quantity)
        {
            ProductId = productId;
            Name = name ?? throw new ArgumentNullException(nameof(name), "Name cannot be null");
            Price = price;
            Quantity = quantity;
        }
    }
}