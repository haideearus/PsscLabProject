using PsscFinalProject.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace PsscFinalProject.Api.Models
{
    public class InputProduct
    {
        [Required]
        [RegularExpression(ClientEmail.Pattern)]
        public required ClientEmail Client_Email { get; set; }

        [Required]
        [RegularExpression(ProductCode.Pattern)]
        public required ProductCode ProdCode { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Qunatity { get; set; }
    }
}
