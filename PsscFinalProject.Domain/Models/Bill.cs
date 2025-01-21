using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record Bill
    {
        public int BillId { get; set; } 
        public int OrderId { get; set; }   
        public DateTime BillDateTime { get; set; }
        public BillNumber? BillNumber { get; set; }
        public ProductPrice? Amount { get; set; } 
    }
}
