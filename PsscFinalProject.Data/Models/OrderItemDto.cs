using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models;

public partial class OrderItemDto
{
    [JsonIgnore]
    public int OrderItemId { get; set; }

    public string ProductCode { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [Key]
    public int Nr_crt { get; set; }
    //public OrderDto Order { get; set; } = default!;
    //public ProductDto Product { get; set; } = default!;
}
