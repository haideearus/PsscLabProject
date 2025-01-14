using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models;

public partial class BillDto
{
    [JsonIgnore]
    [Key]
    public int BillId { get; set; }

    public int OrderId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime BillingDate { get; set; }

    [JsonIgnore]
    public virtual OrderDto? Order { get; set; }
}
