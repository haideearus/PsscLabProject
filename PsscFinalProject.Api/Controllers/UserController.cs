using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync(); // Ensure "Users" exists in DbContext
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id); // Ensure "Users" exists in DbContext
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
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
                await _context.Users.AddAsync(userDto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok($"User with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
