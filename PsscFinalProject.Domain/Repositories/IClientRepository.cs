
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public interface IClientRepository
    {
        Task<bool> ClientExistsAsync(string clientEmail);

        Task<List<ClientEmail>> GetExistingClientsAsync(IEnumerable<string> clientsToCheck);
    }
}
