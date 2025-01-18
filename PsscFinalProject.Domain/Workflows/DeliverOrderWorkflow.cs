//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PsscFinalProject.Domain.Workflows
//{
//    internal class DeliverOrderWorkflow
//    {
//        private readonly ApiService _apiService;
//    }
//    public async Task ProcessOrderAsync(int orderId)
//        {
//            var order = await _apiClient.GetOrderAsync(orderId);
//            if (!ValidateCustomer(order.Customer))
//            {
//                Console.WriteLine("Validation failed: Customer data incomplete.");
//                return;
//            }
//            Console.WriteLine("Customer data validated.");

//            Console.WriteLine("Starting packaging process...");
//            await SimulateTask("Packaging");
//            Console.WriteLine("Packaging verified.");

//            var packageDetails = new PackageDetails { Dimensions = "30x20x10", Weight = 5.0 };
//            order.PackageDetails = packageDetails;
//            Console.WriteLine("Notifying courier...");
//            var awb = NotifyCourier(packageDetails);

//            Console.WriteLine("Waiting for courier confirmation...");
//            await Task.Delay(2000); // Simulate wait time
//            Console.WriteLine($"Courier confirmed. AWB: {awb}");

//            await _apiClient.AddAWBToOrderAsync(orderId, awb);
//            await _apiClient.UpdateOrderStatusAsync(orderId, OrderStatus.InTransit);
//            Console.WriteLine("Tracking order...");

//            Console.WriteLine("Order delivered. Generating warranty...");
//            var warranty = GenerateWarranty("electronics");
//            Console.WriteLine($"Warranty: {warranty}");

//            NotifyCustomer(order.Customer, "Order delivered and warranty issued.");
//        }

//        private bool ValidateCustomer(Customer customer)
//        {
//            return !string.IsNullOrEmpty(customer.FirstName) &&
//                   !string.IsNullOrEmpty(customer.LastName) &&
//                   !string.IsNullOrEmpty(customer.Email) &&
//                   !string.IsNullOrEmpty(customer.PhoneNumber);
//        }

//        private async Task SimulateTask(string taskName)
//        {
//            Console.WriteLine($"{taskName} in progress...");
//            await Task.Delay(2000); // Simulate delay
//            Console.WriteLine($"{taskName} completed.");
//        }

//        private string NotifyCourier(PackageDetails packageDetails)
//        {
//            Console.WriteLine($"Sending package details to courier: Dimensions={packageDetails.Dimensions}, Weight={packageDetails.Weight}kg.");
//            return Guid.NewGuid().ToString(); // Simulated AWB
//        }

//        private string GenerateWarranty(string productType)
//        {
//            return productType switch
//            {
//                "food" => "2 days",
//                "electronics" => "2 years",
//                _ => "30 days"
//            };
//        }

//        private void NotifyCustomer(Customer customer, string message)
//        {
//            Console.WriteLine($"Sending notification to {customer.Email}: {message}");
//        }
//    }
//}
