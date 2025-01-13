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
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var users = _userService.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUser(int id)
        {
            var user = _userService.Users.FindAsync(id);
            if (user.IsCompleted)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<UserDto> CreateUser([FromBody] UserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdUser = _userService.Users.AddAsync(newUser);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Username }, createdUser);
        }

        //// PUT: api/users/{id}
        //[HttpPut("{id}")]
        //public IActionResult UpdateUser(int id, [FromBody] UserDto updatedUser)
        //{
        //    if (id != updatedUser.Email)
        //    {
        //        return BadRequest("User ID mismatch.");
        //    }

        //    if (!_userService.UpdateUser(id, updatedUser))
        //    {
        //        return NotFound($"User with ID {id} not found.");
        //    }

        //    return NoContent();
        //}

        //// DELETE: api/users/{id}
        //[HttpDelete("{id}")]
        //public IActionResult DeleteUser(int id)
        //{
        //    if (!_userService.DeleteUser(id))
        //    {
        //        return NotFound($"User with ID {id} not found.");
        //    }

        //    return NoContent();
        //}
    }
}
