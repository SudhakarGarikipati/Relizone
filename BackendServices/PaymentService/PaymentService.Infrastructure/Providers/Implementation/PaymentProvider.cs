using AutoMapper;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.Dtos;
using PaymentService.Infrastructure.Providers.Abstraction;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace PaymentService.Infrastructure.Providers.Implementation
{
    public class PaymentProvider : IPaymentProvider
    {
        readonly IMapper _mapper;
        readonly RazorpayClient _client;
        readonly IConfiguration _configuration;

        public PaymentProvider(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration; 
            this._client = new RazorpayClient(_configuration["RazorPay:Key"], _configuration["RazorPay:Secret"]);
        }

        public Payment GetPaymentDetails(string paymentId)
        {
            return _client.Payment.Fetch(paymentId);
        }

        public string CreateOrder(RazorPayOrderDto orderDto)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", orderDto.Amount); // amount in the smallest currency unit
            options.Add("receipt", orderDto.Receipt);
            options.Add("currency", orderDto.Currency);
            Order order = _client.Order.Create(options);
            return order["id"].ToString();
        }

        public string VerifyPayment(PaymentConfirmationDto payment)
        {
            string payload = string.Format($"{payment.PaymentId}|{payment.OrderId}");
            string secret = RazorpayClient.Secret;
            string signature = getActualSignature(payload, secret);
            bool status = signature.Equals(payment.Signature);
            if (status)
            {
                Payment paymentDetails = GetPaymentDetails(payment.PaymentId);
                return paymentDetails["status"].ToString();
            }
            return string.Empty;
        }

        private string getActualSignature(string payload, string secret)
        {
            byte[] secretBytes = StringEncode(secret);
            HMACSHA256 hmac = new HMACSHA256(secretBytes);
            byte[] payloadBytes = StringEncode(payload);
            return HashEncode(hmac.ComputeHash(payloadBytes));
        }

        private static string HashEncode(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private static byte[] StringEncode(string secret)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(secret);
        }
    }
}
