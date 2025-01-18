//using PsscFinalProject.Data.Models;
//using PsscFinalProject.Domain.Models;
//using System.Text.Json;

//namespace PsscFinalProject.Api.Service
//{
//    public class ApiService
//    {
//        private readonly HttpClient _httpClient;

//        public ApiService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }
//        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
//        {
//            var response = await _httpClient.GetAsync($"api/orders/{orderId}");
//            if (response.IsSuccessStatusCode)
//            {
//                var content = await response.Content.ReadAsStringAsync();
//                return JsonSerializer.Deserialize<OrderDto>(content);
//            }
//            return null;
//        }

//        public async Task UpdateOrderAsync(Order order)
//        {
//            var content = new StringContent(JsonSerializer.Serialize(order), System.Text.Encoding.UTF8, "application/json");
//            await _httpClient.PutAsync($"api/orders/{order.Id}", content);
//        }
//    }
//}
