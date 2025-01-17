using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using System.Threading.Tasks;
using System;

public class DeliverOrderWorkflow
{
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliverOrderWorkflow(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public async Task ProcessOrderAsync(int orderId)
    {
        // Obținem comanda din baza de date
        var order = await _deliveryRepository.GetOrderByIdAsync(orderId);

        // Obținem detalii clientului pe baza email-ului clientului din comandă
        var client = await _deliveryRepository.GetClientByEmailAsync(order.ClientEmail);

        // Validăm informațiile clientului
        if (!ValidateClient(client))
        {
            Console.WriteLine("Validation failed: Client data incomplete.");
            return;
        }

        Console.WriteLine("Client data validated.");

        // Simulăm procesul de ambalare
        Console.WriteLine("Starting packaging process...");
        await SimulateTask("Packaging");
        Console.WriteLine("Packaging verified.");

        // Creăm detaliile ambalajului și le adăugăm la comandă
        var packageDetails = new PackageDetails { Dimensions = "30x20x10", Weight = 5.0 };
        await _deliveryRepository.AddPackageDetailsToOrderAsync(orderId, packageDetails);

        // Notificăm curierul
        Console.WriteLine("Notifying courier...");
        var awb = NotifyCourier(packageDetails);

        Console.WriteLine("Waiting for courier confirmation...");
        await Task.Delay(2000); // Simulăm o perioadă de așteptare
        Console.WriteLine($"Courier confirmed. AWB: {awb}");

        // Actualizăm comanda cu noul status
        await _deliveryRepository.UpdateOrderStatusAsync(orderId, OrderStatus.InTransit);

        Console.WriteLine("Order is in transit...");

        // Generăm garanția
        Console.WriteLine("Generating warranty...");
        var warranty = GenerateWarranty("electronics");
        Console.WriteLine($"Warranty: {warranty}");

        // Notificăm clientul
        NotifyClient(client, "Order delivered and warranty issued.");
    }


    private bool ValidateClient(Client client)
    {
        return !string.IsNullOrEmpty(client.FirstName) &&
               !string.IsNullOrEmpty(client.LastName) &&
               !string.IsNullOrEmpty(client.Email) &&
               !string.IsNullOrEmpty(client.PhoneNumber);
    }

    private async Task SimulateTask(string taskName)
    {
        Console.WriteLine($"{taskName} in progress...");
        await Task.Delay(2000); 
        Console.WriteLine($"{taskName} completed.");
    }

    private string NotifyCourier(PackageDetails packageDetails)
    {
        Console.WriteLine($"Sending package details to courier: Dimensions={packageDetails.Dimensions}, Weight={packageDetails.Weight}kg.");
        return Guid.NewGuid().ToString(); 
    }

    private string GenerateWarranty(string productType)
    {
        return productType switch
        {
            "food" => "2 days",
            "electronics" => "2 years",
            _ => "30 days"
        };
    }

    private void NotifyClient(Client client, string message)
    {
        Console.WriteLine($"Sending notification to {client.Email}: {message}");
    }
}