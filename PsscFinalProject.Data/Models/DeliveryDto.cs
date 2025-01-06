using System;
using System.Collections.Generic;

namespace PsscFinalProject.Data.Models;

public partial class DeliveryDto
{
    public int DeliveryId { get; set; }

    public int OrderId { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string? TrackingNumber { get; set; }

    public int? Courier { get; set; }

    public virtual OrderDto Order { get; set; } = null!;
}
