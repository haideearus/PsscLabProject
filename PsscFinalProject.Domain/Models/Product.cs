using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public class Product
    {
        // Primary Key
        public int ProductId { get; set; }

        // Product Name
        public string? Name { get; set; }

        // Product Code (Unique identifier)
        public string? Code { get; set; }

        // Product Price
        public decimal Price { get; set; }

        // Quantity Type (e.g., Units, Box, etc.)
        public string? QuantityType { get; set; }

        // Stock Quantity
        public int Stock { get; set; }

        // Optional: Navigation property if there's a relationship with another entity (e.g., Category)
        // public virtual Category Category { get; set; }
    }
}
