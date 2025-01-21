using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IAddressRepository
    {
        Task AddAddressAsync(ClientEmail clientEmail, ShippingAddress address);
        Task ModifyAddressAsync(ClientEmail clientEmail, int addressId, ShippingAddress newAddress);
        Task<List<ShippingAddress>> GetAddressesByClientEmailAsync(ClientEmail clientEmail);
    }
}
