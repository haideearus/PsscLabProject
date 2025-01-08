using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Data;
using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PsscDbContext _context;

        public UserController(PsscDbContext context)
        {
            _context = context;
        }

        // POST: api/users/add
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data cannot be null.");
            }

            try
            {
                // Check if the email already exists in the database
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
                if (existingUser != null)
                {
                    return Conflict($"A user with the email {userDto.Email} already exists.");
                }

                // Add the new user
                await _context.Users.AddAsync(userDto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUserByEmail), new { email = userDto.Email }, userDto);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest($"Database update failed: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync(); // Ensure "Users" exists in DbContext
            return Ok(users);
        }

        // GET: api/users/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found.");
            }
            return Ok(user);
        }


        // DELETE: api/users/{email}
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _context.Users.FindAsync(email);
            if (user == null)
            {
                return NotFound($"User with ID {email} not found.");
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok($"User with ID {email} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
