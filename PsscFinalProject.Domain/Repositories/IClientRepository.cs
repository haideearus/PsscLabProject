
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public interface IClientRepository
    {
        Task<List<ClientEmail>> GetExistingClientsAsync(IEnumerable<string> clientsToCheck);
    }
}
