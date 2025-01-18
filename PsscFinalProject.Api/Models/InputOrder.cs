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
}