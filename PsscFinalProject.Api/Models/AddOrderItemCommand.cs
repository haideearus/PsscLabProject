using System.ComponentModel.DataAnnotations;

namespace PsscFinalProject.Api.Models
{
    public class AddOrderItemCommand
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public required string ProductCode { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
