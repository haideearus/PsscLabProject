using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PsscFinalProject.Data.Models;

public partial class OrderitemDto
{
    [Key]
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual OrderDto Order { get; set; } = null!;

    public virtual ProductDto Product { get; set; } = null!;
}
