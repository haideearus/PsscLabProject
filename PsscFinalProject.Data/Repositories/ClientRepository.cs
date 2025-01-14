using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly PsscDbContext dbContext;

        public ClientRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> ClientExistsAsync(string email)
        {
            return await dbContext.Clients.AsNoTracking().AnyAsync(c => c.Email == email);
        }
        //public Task<bool> ClientExistsAsync(string clientEmail)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<ClientEmail>> GetExistingClientsAsync(IEnumerable<string> clientsToCheck)
        {
            // Fetch clients based on the provided email list
            List<ClientDto> clients = await dbContext.Clients
                .Where(client => clientsToCheck.Contains(client.Email))
                .AsNoTracking()
                .ToListAsync();

            // Map the fetched clients to domain models (ClientEmail)
            return clients.Select(client => new ClientEmail(client.Email))
                          .ToList();
        }
    }
}
