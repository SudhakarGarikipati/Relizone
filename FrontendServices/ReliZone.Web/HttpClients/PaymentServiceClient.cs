using ReliZone.Web.Models;
using System.Text;
using System.Text.Json;

namespace ReliZone.Web.HttpClients
{
    public class PaymentServiceClient
    {
        HttpClient _httpClient;

        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        // Implement methods to interact with the Payment Service API

        public async Task<string> CreateOrderAsync(RazorPayOrderModel razorPayOrderModel)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(razorPayOrderModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("payment/createorder", content);
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error creating order: {response.ReasonPhrase}");
            }
        }

        public async Task<string> VerifyPaymentAsync(PaymentConfirmModel payment)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("paymnet/VerifyPayment", content);
            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                return resultContent;
            }
            else
            {
                throw new Exception($"Error verifying payment: {response.ReasonPhrase}");
            }
        }

        public async Task<bool> SavePaymentDetailsAsync(TransactionModel transaction)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(transaction), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("payment/SavePaymentDetails", content);

            if (response.IsSuccessStatusCode)
            {
                var strResult = await response.Content.ReadAsStringAsync();
                return bool.Parse(strResult);
            }
            return false;
        }
    }
}
