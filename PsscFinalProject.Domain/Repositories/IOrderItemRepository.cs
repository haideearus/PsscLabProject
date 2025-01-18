using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderProducts;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IOrderItemRepository
    {
            Task <List<CalculatedProduct>> GetProductsAsync ();
            Task SaveOrders(PaidOrderProducts order);
     
    }
}
