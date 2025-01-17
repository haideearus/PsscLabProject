using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using System.Text.Json;

namespace PsscFinalProject.Api
{
    public class ApiServiceDelivery
    {
        private readonly HttpClient _httpClient;

        public ApiServiceDelivery(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var response = await _httpClient.GetAsync($"api/orders/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    return JsonSerializer.Deserialize<OrderDto>(content);
                }
                catch (JsonException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            return null;
        }

        public async Task UpdateOrderAsync(OrderDto order)
        {
            var content = new StringContent(JsonSerializer.Serialize(order), System.Text.Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"api/orders/{order.OrderId}", content);
        }
    }
}
