using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public class UserRepository
    {
        private readonly PsscDbContext dbContext;

        public UserRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task SaveUsersAsync(IEnumerable<UserDto> users)
        {
            dbContext.Users.AddRange(users.Where(u => u.UserId == 0));
            dbContext.Users.UpdateRange(users.Where(u => u.UserId > 0));
            await dbContext.SaveChangesAsync();
        }
    }
}
