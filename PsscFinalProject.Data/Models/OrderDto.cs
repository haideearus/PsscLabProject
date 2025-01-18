using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models;

public partial class OrderDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public int PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public int? State { get; set; }

    public string ClientEmail { get; set; } = null!;

    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    [JsonIgnore]
    public virtual ICollection<BillDto> Bills { get; set; } = new List<BillDto>();

    [JsonIgnore]
    public virtual ClientDto? Client { get; set; }

    [JsonIgnore]
    public virtual ICollection<DeliveryDto> Deliveries { get; set; } = new List<DeliveryDto>();
}
