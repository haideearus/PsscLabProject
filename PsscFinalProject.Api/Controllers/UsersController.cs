using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Data;
using PsscFinalProject.Data.Models;

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

        // GET: api/users/{id}
        [HttpGet("{username}")]
        public ActionResult<UserDto> GetUser(string username)
        {
            var user = _userService.Users.FindAsync(username);
            Console.WriteLine(user.Result);
            if (user.IsCompleted)
            {
                return NotFound($"User with ID {username} not found.");
            }
            return Ok(username);
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
