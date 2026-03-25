using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Dtos;
using PaymentService.Application.Services.Abstraction;
using PaymentService.Infrastructure.Providers.Abstraction;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController :  ControllerBase
    {
        readonly IPaymentProvider _paymentProvider;
        readonly IPaymentAppService _paymentAppService;

        public PaymentController(IPaymentProvider paymentProvider, IPaymentAppService paymentAppService)
        {
            this._paymentProvider = paymentProvider;
            this._paymentAppService = paymentAppService;
        }

        [HttpPost]
        public IActionResult CreateOrder(RazorPayOrderDto order)
        {
            var orderId = _paymentProvider.CreateOrder(order);
            if (orderId != null)
            {
                return Ok(orderId);
            }
            return BadRequest("Unable to create order");
        }

        [HttpPost]
        public IActionResult VerifyPayment(PaymentConfirmationDto payment)
        {
            var status = _paymentProvider.VerifyPayment(payment);
            return Ok(status);
        }

        [HttpPost]
        public IActionResult SavePaymentDetails(PaymentDetailDto paymentDetail)
        {
            var status = _paymentAppService.SavePaymentDetails(paymentDetail);
            return Ok(status);
        }
    }
}
