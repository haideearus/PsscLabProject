using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace PsscFinalProject.Data.Repositories
{
    public class UserRepository
    {
        private readonly PsscDbContext dbContext;

        public UserRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserDto>> GetUsersPaginatedAsync(int pageIndex, int pageSize)
        {
            return await dbContext.Users
                .AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task SaveUsersAsync(IEnumerable<UserDto> users)
        {
            try
            {
                dbContext.Users.AddRange(users.Where(u => u.UserId == 0));
                dbContext.Users.UpdateRange(users.Where(u => u.UserId > 0));
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it based on your business logic
                throw new Exception("An error occurred while saving users.", ex);
            }
        }

    }
}
