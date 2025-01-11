using System.ComponentModel.DataAnnotations;
using PsscFinalProject.Domain.Models;

namespace PsscFinalProject.Api.Models
{
    public class InputOrder
    {
        [Required]
        //[RegularExpression(ClientEmail.Pattern.ToString(), ErrorMessage = "Invalid email format.")] // Use ToString() to get the pattern string
        public required string ClientEmail { get; set; } // Add 'required' modifier for non-nullable property

        [Required]
        public required List<InputProduct> ProductList { get; set; } // Add 'required' modifier for non-nullable property
    }

    public class InputProduct
    {

        [Required]
       // [RegularExpression(ProductCode.Pattern.ToString(), ErrorMessage = "Invalid product code format.")] // Use ToString() to get the pattern string
        public required string ProductId { get; set; } // Add 'required' modifier for non-nullable property

        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        public int Quantity { get; set; } // 'int' is already non-nullable
    }
}
