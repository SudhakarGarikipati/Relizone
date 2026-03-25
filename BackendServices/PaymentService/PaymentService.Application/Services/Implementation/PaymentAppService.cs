using AutoMapper;
using PaymentService.Application.Dtos;
using PaymentService.Application.Repositories;
using PaymentService.Application.Services.Abstraction;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Services.Implementation
{

    public class PaymentAppService : IPaymentAppService
    {
        readonly IPaymentServiceRepository _paymentRepository;
        readonly IMapper _mapper;

        public PaymentAppService(IPaymentServiceRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            this._mapper = mapper;
        }
       
        public bool SavePaymentDetails(PaymentDetailDto paymentDetailDto)
        {
            var paymentDetail = _mapper.Map<PaymentDetail>(paymentDetailDto);
            return _paymentRepository.SavePaymentDetails(paymentDetail);
        }
    }
}
