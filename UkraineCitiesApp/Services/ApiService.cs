using System.Net.Http.Json;
using UkraineCitiesApp.Models;

namespace UkraineCitiesApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        // Використовуйте "10.0.2.2" для емулятора Android, щоб звертатися до вашого комп'ютера
        private const string BaseUrl = "http://10.0.2.2:5229/api/";

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<List<City>> GetCitiesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<City>>("Cities/get-cities");
                if (response == null)
                {
                    throw new Exception("No cities were returned from the server.");
                }
                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw new Exception("Error connecting to the server. Make sure the API is running.");
            }
        }

        public async Task<List<City>> GetSampleCitiesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<City>>("Cities/get-sample-cities");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw new Exception("Error connecting to the server. Make sure the API is running.");
            }
        }

        public async Task<MinMaxDistance> GetMinMaxDistanceAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MinMaxDistance>("Cities/get-min-max-distance");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw new Exception("Error connecting to the server. Make sure the API is running.");
            }
        }

        public async Task<List<ContactModel>> GetContactsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ContactModel>>("Contacts/get-contacts");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw new Exception("Error connecting to the server. Make sure the API is running.");
            }
        }

        public async Task<List<ContactModel>> GetSampleContactsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ContactModel>>("Contacts/get-sample-contacts");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw new Exception("Error connecting to the server. Make sure the API is running.");
            }
        }
    }
}
