using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PsscFinalProject.Data.Models;

public partial class OrderDto
{

    [Key]
    public int OrderId { get; set; }

    public int ClientId { get; set; }

    public DateTime OrderDate { get; set; }

    public int PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public int? State { get; set; }

    public virtual ICollection<BillDto> Bills { get; set; } = new List<BillDto>();

    public virtual ClientDto? Client { get; set; }

    public virtual ICollection<DeliveryDto> Deliveries { get; set; } = new List<DeliveryDto>();
}
