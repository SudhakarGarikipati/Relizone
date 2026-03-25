using PaymentService.Application.Repositories;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Persistance.Repositories
{
    public class PaymentServiceRepository : IPaymentServiceRepository
    {
        PaymentServiceDbContext _paymentServiceDbContext;

        public PaymentServiceRepository(PaymentServiceDbContext paymentServiceDbContext)
        {
            _paymentServiceDbContext = paymentServiceDbContext;
        }

        public bool SavePaymentDetails(PaymentDetail paymentDetail)
        {
            if (paymentDetail != null) {
                _paymentServiceDbContext.PaymentDetails.Add(paymentDetail);
                var result = _paymentServiceDbContext.SaveChanges();
                return result > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
