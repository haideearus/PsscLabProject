using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Domain.Models;

namespace PsscFinalProject.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PsscDbContext dbContext;

        public UserRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<ClientEmail>> GetExistingUsersAsync(IEnumerable<string> usersToCheck)
        {

            List<UserDto> users = await dbContext.Users
                .Where(users => usersToCheck.Contains(users.Email))
                .AsNoTracking()
                .ToListAsync();

            return users.Select(users => new ClientEmail(users.Email))
                          .ToList();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            var userDtos = await dbContext.Users.AsNoTracking().ToListAsync();

            // Mapează UserDto la User
            var users = userDtos.Select(dto => new User
            {
                UserName = dto.Username,
                Password = dto.Password,
                // Adaugă alte câmpuri necesare aici
            }).ToList();
            return users;
        }


    }
}
