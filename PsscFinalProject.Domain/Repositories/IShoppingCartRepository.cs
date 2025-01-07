using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetAsync(int cartId);
        Task<ShoppingCart> GetByUserIdAsync(int userId);
        Task SaveAsync(ShoppingCart cart);
    }

}
