using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Data;
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

    [HttpPost("add")]
    public async Task<IActionResult> AddClient([FromBody] ClientDto clientDto)
    {
        if (clientDto == null)
        {
            return BadRequest("Client data cannot be null.");
        }

        // Check if the user exists with the provided email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == clientDto.Email);
        if (user == null)
        {
            return BadRequest($"No user found with email {clientDto.Email}. A user must exist before creating a client.");
        }

        // Check if a client already exists with the same email
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Email == clientDto.Email);
        if (existingClient != null)
        {
            return Conflict($"Client already exists for user with email {clientDto.Email}.");
        }

        // Set the EmailNavigation property to link the client to the existing user
        clientDto.EmailNavigation = user;

        try
        {
            // Add the new client
            await _context.Clients.AddAsync(clientDto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientById), new { id = clientDto.ClientId }, clientDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
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
