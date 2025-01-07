using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public interface IClientRepository
    {
        // In IClientRepository
        Task<Client> GetByUserIdAsync(string userId);
        // In IClientRepository
        Task SaveAsync(Client client);

        Task GetClientByIdAsync(object clientId);
        Task<List<ClientDto>> GetClientsAsync();
        Task SaveClientsAsync(IEnumerable<ClientDto> clients);
        Task<Client> GetByUserIdAsync(int userId);
    }
}
