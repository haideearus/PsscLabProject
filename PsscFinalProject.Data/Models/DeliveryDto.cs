using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PsscFinalProject.Data.Models;

namespace PsscFinalProject.Data.Models;

public partial class DeliveryDto
{
    [JsonIgnore]
    [Key]
    public int DeliveryId { get; set; }

    public int OrderId { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string? TrackingNumber { get; set; }

    public string? Courier { get; set; }

    [JsonIgnore]
    public virtual OrderDto? Order { get; set; }
}