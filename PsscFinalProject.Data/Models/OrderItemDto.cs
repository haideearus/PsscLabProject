using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models;

public partial class OrderItemDto
{
    [JsonIgnore]
    [Key]
    public int OrderItemId { get; set; }

    public string ProductCode { get; set; } = null!;

    public decimal Price { get; set; }
}
