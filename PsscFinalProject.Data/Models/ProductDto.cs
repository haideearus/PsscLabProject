using System;
using System.Collections.Generic;

namespace PsscFinalProject.Data.Models;

public partial class ProductDto
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public decimal Price { get; set; }

    public string QuantityType { get; set; } = null!;

    public int Stock { get; set; }

    public virtual ICollection<OrderitemDto> Orderitems { get; set; } = new List<OrderitemDto>();
}
