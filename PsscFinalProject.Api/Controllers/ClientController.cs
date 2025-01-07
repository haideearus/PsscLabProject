using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly PsscDbContext _context;

    public ClientController(PsscDbContext context)
    {
        _context = context;
    }

    // GET: api/clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
    {
        var clients = await _context.Clients.ToListAsync();
        return Ok(clients);
    }

    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientDto clientDto)
    {
        // Validate input
        if (string.IsNullOrEmpty(clientDto.Email))
        {
            return BadRequest("Email is required to associate the client with a user.");
        }

        // Find the user by email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == clientDto.Email);
        if (user == null)
        {
            return BadRequest("No user found with the provided email address.");
        }

        // Associate the client with the found user
        var newClient = new ClientDto
        {
            FirstName = clientDto.FirstName,
            LastName = clientDto.LastName,
            Email = clientDto.Email,
            PhoneNumber = clientDto.PhoneNumber,
            UserId = user.UserId, // Associate the client with the user's ID
            User = user // Optional: Set the navigation property
        };

        // Add the client to the database
        _context.Clients.Add(newClient);
        await _context.SaveChangesAsync();

        // Return the created client
        return CreatedAtAction(nameof(GetClientById), new { id = newClient.ClientId }, newClient);
    }


    // GET: api/clients/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var clientDto = await _context.Clients.FindAsync(id);
        if (clientDto == null)
        {
            return NotFound();
        }

        return Ok(clientDto);
    }

    // GET: api/clients/search?nume=John&prenume=Doe
    [HttpGet("search")]
    public async Task<IActionResult> SearchClient([FromQuery] string nume, [FromQuery] string prenume)
    {
        if (string.IsNullOrEmpty(nume) || string.IsNullOrEmpty(prenume))
        {
            return BadRequest("First name (nume) and last name (prenume) are required.");
        }

        var matchingClients = await _context.Clients
            .Where(c => c.FirstName == nume && c.LastName == prenume)
            .ToListAsync();

        if (matchingClients == null || matchingClients.Count == 0)
        {
            return NotFound("No clients found with the specified name.");
        }

        return Ok(matchingClients);
    }

    // PUT: api/clients/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDto updatedClientDto)
    {
        if (id != updatedClientDto.ClientId)
        {
            return BadRequest("Client ID mismatch.");
        }

        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound($"Client with ID {id} not found.");
        }

        // Update client properties
        client.FirstName = updatedClientDto.FirstName;
        client.LastName = updatedClientDto.LastName;
        client.Email = updatedClientDto.Email;
        client.PhoneNumber = updatedClientDto.PhoneNumber;
        client.UserId = updatedClientDto.UserId;

        try
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/clients/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound($"Client with ID {id} not found.");
        }

        try
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
