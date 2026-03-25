//using CartService.Application.Dtos;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;

//namespace CartService.Application.HttpClients
//{
//    public class CatalogServiceClient
//    {
//        readonly HttpClient _httpClient;

//        public CatalogServiceClient(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<IEnumerable<ProductDto>> GetProductsByIds(int[] productIds)
//        {

//            StringContent content = new StringContent(JsonSerializer.Serialize(productIds));
//            var response = await _httpClient.PostAsync("catalog/getbyids", content);
//            if (response.IsSuccessStatusCode)
//            {
//                string data = await response.Content.ReadAsStringAsync();
//                if (data != null)
//                {
//                    return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(data, new JsonSerializerOptions
//                    {
//                        PropertyNameCaseInsensitive = true
//                    });
//                }
//            }
//            else
//            {
//                throw new Exception($"Error fetching catalog items: {response.ReasonPhrase}");
//            }
//            return [];
//        }
//    }
//}
using CartService.Application.Dtos;
using System.Text;
using System.Text.Json;

namespace CartService.Application.HttpClients
{
    public class CatalogServiceClient
    {
        private readonly HttpClient _httpClient;

        public CatalogServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByIds(int[] productIds)
        {
            if (productIds == null || productIds.Length == 0)
                return Array.Empty<ProductDto>();

            using var content = new StringContent(
                JsonSerializer.Serialize(productIds),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("api/catalog/getbyids", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"Error fetching catalog items: {response.StatusCode} - {response.ReasonPhrase}. Response: {errorBody}"
                );
            }

            var data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(data))
                return Array.Empty<ProductDto>();

            return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Array.Empty<ProductDto>();
        }
    }
}
