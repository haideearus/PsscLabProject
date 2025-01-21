using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PsscFinalProject.Data;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Repositories;

namespace WebUI.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CheckUserExistance(string username, string password)
        {
            try
            {
                var existingUsers = await _userRepository.GetAllUsersAsync();
                
                foreach (var user in existingUsers)
                {
                    Console.WriteLine(user.UserName);
                    Console.WriteLine(user.Password);
                    Console.WriteLine(user.Email);
                    if (!user.UserName.Equals(username)) continue;
                    if (user.Password.Equals(password))
                    {
                        CartService.SetEmail(user.Email);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la verificarea utilizatorului: {ex.Message}");
                throw;
            }
        }
    }
}
