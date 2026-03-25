using ReliZone.Web.Messages;

namespace ReliZone.Web.HttpClients
{
    public class OrderServiceClient
    {
        readonly HttpClient _httpClient;

        public OrderServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateOrder(OrderMessage orderModel)
        {
            StringContent model = new StringContent(System.Text.Json.JsonSerializer.Serialize(orderModel), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/order/CreateOrder/", model);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
