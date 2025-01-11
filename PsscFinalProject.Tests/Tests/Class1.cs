using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;

namespace PsscFinalProject.Tests.Tests
{
    [TestFixture]
    public class ApiTest
    {
        private RestClient _client;

        [SetUp]
        public void Setup()
        {
            // Inițializează clientul RestSharp
            _client = new RestClient("https://localhost:7195"); // Schimbă cu URL-ul API-ului tău
        }

        [Test]
        public async Task GetData_ShouldReturn200_OK()
        {
            // Creează o cerere GET către endpoint-ul API-ului
            var request = new RestRequest("/api/client", Method.Get); // Schimbă cu endpoint-ul tău
            
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine(response.StatusCode);
            // Verifică că răspunsul are status 200 OK
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(200));
            //AreEqual(200, (int)response.StatusCode, "Statusul răspunsului nu este 200 OK");

            // Verifică că răspunsul conține datele așteptate
            NUnit.Framework.Assert.That(response.Content, Is.Empty,"Raspuns gol");
                //IsNotEmpty(response.Content, "Răspunsul este gol");
        }
        /*
        [Test]
        public async Task PostData_ShouldReturn201_Created()
        {
            // Creează o cerere POST pentru a trimite date la API
            var request = new RestRequest("data", Method.Post); // Schimbă cu endpoint-ul tău
            request.AddJsonBody(new { name = "John", age = 30 }); // Exemplu de date trimise

            var response = await _client.ExecuteAsync(request);

            // Verifică că răspunsul are status 201 Created
            Assert.AreEqual(201, (int)response.StatusCode, "Statusul răspunsului nu este 201 Created");
        }

        [Test]
        public async Task GetData_InvalidEndpoint_ShouldReturn404_NotFound()
        {
            // Creează o cerere GET către un endpoint invalid
            var request = new RestRequest("invalid-endpoint", Method.GET); // Schimbă cu endpoint invalid
            var response = await _client.ExecuteAsync(request);

            // Verifică că răspunsul are status 404 Not Found
            Assert.AreEqual(404, (int)response.StatusCode, "Statusul răspunsului nu este 404 Not Found");
        }*/
    }
        
}
