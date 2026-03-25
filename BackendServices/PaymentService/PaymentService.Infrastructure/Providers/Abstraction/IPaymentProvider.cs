using PaymentService.Application.Dtos;
using Razorpay.Api;

namespace PaymentService.Infrastructure.Providers.Abstraction
{
    public interface IPaymentProvider
    {
        string CreateOrder(RazorPayOrderDto orderDto);

        Payment GetPaymentDetails(string paymentId);

        string VerifyPayment(PaymentConfirmationDto paymentConfirmation);
    }
}
