using PaymentService.Application.Dtos;

namespace PaymentService.Application.Services.Abstraction
{
    public interface IPaymentAppService
    {
      
        bool SavePaymentDetails(PaymentDetailDto paymentDetail);

    }
}
