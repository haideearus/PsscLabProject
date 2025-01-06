using System;
using System.Collections.Generic;

namespace PsscFinalProject.Data.Models;

public partial class OrderDto
{
    public int OrderId { get; set; }

    public int ClientId { get; set; }

    public DateTime OrderDate { get; set; }

    public int PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public int? State { get; set; }

    public virtual ICollection<BillDto> Bills { get; set; } = new List<BillDto>();

    public virtual ClientDto Client { get; set; } = null!;

    public virtual ICollection<DeliveryDto> Deliveries { get; set; } = new List<DeliveryDto>();

    public virtual ICollection<OrderitemDto> Orderitems { get; set; } = new List<OrderitemDto>();
}
