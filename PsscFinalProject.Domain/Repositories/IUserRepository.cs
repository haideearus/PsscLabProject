
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<ClientEmail>> GetExistingUsersAsync(IEnumerable<string> usersToCheck);
    }
}
