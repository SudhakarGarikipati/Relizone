namespace PaymentService.Application.Dtos
{
    public class PaymentConfirmationDto
    {
        public string PaymentId { get; set; }

        public string OrderId { get; set; }

        public string Signature { get; set; }
    }
}
