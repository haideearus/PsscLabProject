using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly PsscDbContext dbContext;

        public AddressRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAddressAsync(ClientEmail clientEmail, ShippingAddress address, PaymentMethod paymentMethod)
        {
            // Check if the client exists
            var clientExists = await dbContext.Clients.AnyAsync(c => c.Email == clientEmail.Value);
            if (!clientExists)
            {
                throw new InvalidOperationException($"Client with email '{clientEmail.Value}' does not exist.");
            }

            // Create and save the AddressDto
            var addressDto = new AddressDto
            {
                ClientEmail = clientEmail.Value,
                ClientAddress = address.Value,
                PaymentMethod = paymentMethod.ToInt() // Map PaymentMethod to its integer value
            };

            dbContext.Addresses.Add(addressDto);
            await dbContext.SaveChangesAsync();
        }

        public async Task ModifyAddressAsync(ClientEmail clientEmail, int addressId, ShippingAddress newAddress)
        {
            var address = await dbContext.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.ClientEmail == clientEmail.Value);

            if (address == null)
            {
                throw new InvalidOperationException($"Address with ID {addressId} for client '{clientEmail.Value}' does not exist.");
            }

            address.ClientAddress = newAddress.Value;
            dbContext.Addresses.Update(address);
            await dbContext.SaveChangesAsync();
        }

        public async Task ModifyPaymentMethodAsync(ClientEmail clientEmail, int addressId, PaymentMethod newPaymentMethod)
        {
            var address = await dbContext.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.ClientEmail == clientEmail.Value);

            if (address == null)
            {
                throw new InvalidOperationException("Address not found for the specified client.");
            }

            // Update the payment method
            address.PaymentMethod = newPaymentMethod.ToInt(); // Convert to integer for storage
            dbContext.Addresses.Update(address);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<ShippingAddress>> GetAddressesByClientEmailAsync(ClientEmail clientEmail)
        {
            var addressStrings = await dbContext.Addresses
                .Where(a => a.ClientEmail == clientEmail.Value)
                .Select(a => a.ClientAddress)
                .ToListAsync();

            return addressStrings.Select(ShippingAddress.Create).ToList();
        }
    
    }
}
