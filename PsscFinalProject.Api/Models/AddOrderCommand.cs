using System.ComponentModel.DataAnnotations;

public class AddOrderCommand
{
    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public int PaymentMethod { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be greater than zero.")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "ShippingAddress is too long.")]
    public required string ShippingAddress { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Invalid state.")]
    public int State { get; set; }

    [Required]
    [EmailAddress]
    public required string ClientEmail { get; set; }
}
