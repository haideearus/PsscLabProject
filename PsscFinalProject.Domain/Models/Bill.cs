using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public class Bill
    {
        public int BillId { get; set; } // Primary Key
        public int OrderId { get; set; }    // Foreign Key to Order
        public DateTime BillDateTime { get; set; }
        public string BillNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; } 
    }
}
