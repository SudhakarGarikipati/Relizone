namespace OrderService.Application.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }

        public string PaymentId { get; set; }

        public string Street { get; set; }
        public string City { get; set; }

        public string Locality { get; set; }

        public string Phone { get; set; }

        public string ZipCode { get; set; }

        public long UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? AcceptDate { get; set; }

    }
}
