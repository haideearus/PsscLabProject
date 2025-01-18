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

        public async Task<List<ClientEmail>> GetExistingClientsAsync(IEnumerable<string> clientsToCheck)
        {
   
            List<ClientDto> clients = await dbContext.Clients
                .Where(client => clientsToCheck.Contains(client.Email))
                .AsNoTracking()
                .ToListAsync();

            return clients.Select(client => new ClientEmail(client.Email))
                          .ToList();
        }
    }
}
