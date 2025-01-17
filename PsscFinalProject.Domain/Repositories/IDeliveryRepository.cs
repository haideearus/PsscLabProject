
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IDeliveryRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Client> GetClientByEmailAsync(string clientEmail);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task AddPackageDetailsToOrderAsync(int orderId, PackageDetails packageDetails);
        Task SaveChangesAsync();
    }
}