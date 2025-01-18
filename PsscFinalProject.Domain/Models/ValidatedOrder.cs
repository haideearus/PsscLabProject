using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record  ValidatedOrder(ClientEmail ClientEmail, ProductCode ProductCode, ProductQuantity ProductQuantity);
}
