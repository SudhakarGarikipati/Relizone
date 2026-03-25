
using ReliZone.Web.Models;
using System.Text.Json;

namespace ReliZone.Web.HttpClients
{
    public class CatalogServiceClient
    {
        readonly HttpClient _httpClient;

        public CatalogServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductAsync()
        {
            var response = await _httpClient.GetAsync("catalog/getall");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<ProductModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            return [];
        }

    }
}
