using System.Collections.Generic;
using System.Threading.Tasks;
using PsscFinalProject.Data.Models;

namespace PsscFinalProject.Data.Interface
{
    public interface IClientRepository
    {
        Task<bool> ClientExistsAsync(string clientEmail);

        Task<List<ClientEmail>> GetExistingClientsAsync(IEnumerable<string> clientsToCheck);
    }
}
