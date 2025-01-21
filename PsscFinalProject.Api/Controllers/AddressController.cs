using Microsoft.AspNetCore.Mvc;
using PsscFinalProject.Api.Models;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Data.Repositories;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;

[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly IAddressRepository addressRepository;
    private readonly IClientRepository clientRepository;

    public AddressController(IAddressRepository addressRepository, IClientRepository clientRepository)
    {
        this.addressRepository = addressRepository;
        this.clientRepository = clientRepository;
    }

    // Get a list of all clients
    [HttpGet("clients")]
    public async Task<IActionResult> GetClients()
    {
        try
        {
            // Fetch all clients from the database
            var allClientEmails = await clientRepository.GetExistingClientsAsync(new List<string>());
            return Ok(allClientEmails.Select(client => new { Email = client.Value }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAddress([FromBody] AddAddressRequest request)
    {
        try
        {
            // Validate the client exists
            var existingClients = await clientRepository.GetExistingClientsAsync(new List<string> { request.ClientEmail });
            if (!existingClients.Any())
            {
                return NotFound(new { Message = $"Client with email {request.ClientEmail} does not exist." });
            }

            // Validate PaymentMethod
            var paymentMethod = PaymentMethod.FromString(request.PaymentMethod);

            // Validate and add the address
            var clientEmail = new ClientEmail(request.ClientEmail);
            var shippingAddress = ShippingAddress.Create(request.ClientAddress);
            var addressDto = new AddressDto
            {
                ClientEmail = request.ClientEmail,
                ClientAddress = request.ClientAddress,
                PaymentMethod = paymentMethod.ToInt()
            };

            await addressRepository.AddAddressAsync(clientEmail, shippingAddress, paymentMethod);

            return Ok(new { Message = "Address added successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = $"Invalid payment method or address format: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    // Modify an existing address
    [HttpPut("modify")]
    public async Task<IActionResult> ModifyAddress([FromBody] ModifyAddressRequest request)
    {
        try
        {
            var clientEmail = new ClientEmail(request.ClientEmail);
            var newAddress = ShippingAddress.Create(request.NewAddress);

            await addressRepository.ModifyAddressAsync(clientEmail, request.AddressId, newAddress);

            return Ok(new { Message = "Address modified successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = $"Invalid address format: {ex.Message}" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPut("modify/paymentmethod")]
    public async Task<IActionResult> ModifyPaymentMethod([FromBody] ModifyPaymentMethodRequest request)
    {
        try
        {
            // Validate client email
            var clientEmail = new ClientEmail(request.ClientEmail);

            // Modify the payment method for the specified address
            await addressRepository.ModifyPaymentMethodAsync(clientEmail, request.AddressId, request.NewPaymentMethod);

            return Ok(new { Message = "Payment method modified successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = $"Invalid payment method or email format: {ex.Message}" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    // Get all addresses for a client
    [HttpGet("get/{clientEmail}")]
    public async Task<IActionResult> GetAddresses(string clientEmail)
    {
        try
        {
            var clientEmailObj = new ClientEmail(clientEmail);
            var addresses = await addressRepository.GetAddressesByClientEmailAsync(clientEmailObj);

            if (!addresses.Any())
            {
                return NotFound(new { Message = "No addresses found for the specified client." });
            }

            return Ok(addresses.Select(a => a.Value));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}