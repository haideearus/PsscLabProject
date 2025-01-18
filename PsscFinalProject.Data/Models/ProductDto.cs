using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models;

public partial class ProductDto
{
    [JsonIgnore]
    [Key]
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public decimal Price { get; set; }

    public string QuantityType { get; set; } = null!;

    public int Stock { get; set; }

    [JsonIgnore]
    public ICollection<OrderItemDto>? OrderItems { get; set; }
}
