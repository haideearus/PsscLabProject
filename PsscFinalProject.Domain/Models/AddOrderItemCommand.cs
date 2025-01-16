using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public class AddOrderItemCommand
    {
        public class AddOrderItemsCommand
        {
            public int OrderId { get; set; }
            public List<string>? ProductCodes { get; set; }
            public int Quantity { get; set; }
        }

    }
}
