using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PsscFinalProject.Data.Models;

namespace PsscFinalProject.Data.Models;

public partial class BillDto
{
    [Key]
    public int BillId { get; set; }

    public int OrderId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime BillingDate { get; set; }

    public virtual OrderDto? Order { get; set; }
}
