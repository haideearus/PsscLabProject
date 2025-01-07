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

    //// POST: api/clients
    //[HttpPost]
    //public async Task<IActionResult> AddClient([FromBody] ClientDto newClientDto)
    //{
    //    if (newClientDto == null)
    //    {
    //        return BadRequest("Client data cannot be null.");
    //    }

    //    try
    //    {
    //        // Add the new client to the database
    //        await _context.Clients.AddAsync(newClientDto);
    //        await _context.SaveChangesAsync();

    //        return CreatedAtAction(nameof(GetClientById), new { id = newClientDto.ClientId }, newClientDto);
    //    }
    //    catch (DbUpdateException ex)
    //    {
    //        return StatusCode(500, $"Internal server error: {ex.Message}");
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> AddClient(ClientDto clientDto)
    {
        var user = await _context.Users.FindAsync(clientDto.UserId);
        if (user == null)
        {
            return BadRequest("UserId does not correspond to an existing user.");
        }

        clientDto.User = user; // Associate the existing user with the client
        _context.Clients.Add(clientDto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetClientById), new { id = clientDto.ClientId }, clientDto);
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
