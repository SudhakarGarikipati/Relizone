using AutoMapper;

namespace PaymentService.Application.Mappers
{
    public class PaymentMapper:Profile
    {
        public PaymentMapper() {
            CreateMap<Dtos.RazorPayOrderDto, Domain.Entities.PaymentDetail>();
        }
    }
}
