using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PsscFinalProject.Api.Models;
using PsscFinalProject.Data.Models;

namespace WebUI.Services
{

    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Metodă pentru autentificarea unui utilizator
        public async Task<bool> LoginAsync(string username, string password)
        {
            var payload = new
            {
                Username = username,
                Password = password
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/login", content); // Endpoint-ul tău
            return response.IsSuccessStatusCode;
        }

        // Metodă pentru obținerea utilizatorilor
        public async Task<List<UserDto>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("/api/users"); // Endpoint-ul tău
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserDto>>(json);
        }
    }
}
