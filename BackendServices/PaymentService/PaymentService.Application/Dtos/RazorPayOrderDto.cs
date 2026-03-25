namespace PaymentService.Application.Dtos
{
    public class RazorPayOrderDto
    {
        public int Amount { get; set; }

        public string Currency {  get; set; }

        public string Receipt { get; set; }
    }
}
