using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public interface IClientRepository
    {
        Task<List<ClientDto>> GetClientsAsync();
        Task SaveClientsAsync(IEnumerable<ClientDto> clients);
    }
}
