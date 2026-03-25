using PaymentService.Domain.Entities;

namespace PaymentService.Application.Repositories
{
    public interface IPaymentServiceRepository
    {
        bool SavePaymentDetails(PaymentDetail paymentDetail);
    }
}
