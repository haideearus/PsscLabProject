using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task DeleteUserAsync(int userId);
        Task<List<UserDto>> GetUsersPaginatedAsync(int pageIndex, int pageSize);
        Task<List<UserDto>> GetUsersAsync();
        Task SaveUsersAsync(IEnumerable<UserDto> users);
    }
}
