using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public class ProductUI
    {
        public string? ProductName { get; set; }
        public ProductCode? ProductCode { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductQuantityType { get; set; }
        public int? ProductQuantity { get; set; }
    }
}
