using PsscFinalProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Repositories;

namespace PsscFinalProject.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly PsscDbContext dbContext;

        public ClientRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ClientDto> GetClientByIdAsync(int clientId)
        {
            return await dbContext.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(client => client.ClientId == clientId);
        }


        public async Task<List<ClientDto>> GetClientsAsync()
        {
            return await dbContext.Clients.AsNoTracking().ToListAsync();
        }
     
        public async Task SaveClientsAsync(IEnumerable<ClientDto> clients)
        {
            dbContext.Clients.AddRange(clients.Where(c => c.ClientId == 0));
            dbContext.Clients.UpdateRange(clients.Where(c => c.ClientId > 0));
            await dbContext.SaveChangesAsync();
        }
    }
}
