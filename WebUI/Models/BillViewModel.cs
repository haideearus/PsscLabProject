using PsscFinalProject.Domain.Models;

namespace PsscFinalProject.WebUI.Models
{
    public class BillViewModel
    {
        public int BillId { get; set; } // Primary Key
        public int OrderId { get; set; }    // Foreign Key to Order
        public DateTime BillDateTime { get; set; }
        public BillNumber? BillNumber { get; set; }
        public ProductPrice? Amount { get; set; } 
    }
}
