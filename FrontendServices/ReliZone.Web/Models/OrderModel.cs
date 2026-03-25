namespace ReliZone.Web.Models
{
    public class OrderModel
    {
        public string PaymentId { get; set; }

        public long UserId { get; set; }

        public long CartId { get; set; }

        public string Products { get; set; }
    }
}
