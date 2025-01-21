using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Data;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;

namespace PsscFinalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PsscDbContext _userService;

        public UsersController(PsscDbContext userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users =await _userService.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<UserDto>> GetUser(string username)
        {
            var user = await _userService.Users
                                          .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            return Ok(user);
        }

        // POST: api/users/add-user
        [HttpPost("add-user")]
        public async Task<ActionResult> AddUser([FromBody] UserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a user with the same username already exists
            var existingUser = await _userService.Users
                                                 .FirstOrDefaultAsync(u => u.Username == newUser.Username);

            if (existingUser != null)
            {
                return Conflict($"A user with username '{newUser.Username}' already exists.");
            }

            // Create a new User entity
            var userEntity = new UserDto
            {
                Username = newUser.Username,
                Email = newUser.Email,
                Password = newUser.Password // Ensure password is hashed
            };

            // Add the user to the database
            await _userService.Users.AddAsync(userEntity);
            await _userService.SaveChangesAsync();

            return Ok($"User with username '{newUser.Username}' added successfully.");
        }


    // POST: api/users
    [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdUser = await _userService.Users.AddAsync(newUser);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Username }, createdUser);
        }
    }
}
