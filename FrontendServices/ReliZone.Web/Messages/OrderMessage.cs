namespace ReliZone.Web.Messages
{
    public class OrderMessage
    {
        public string PaymentId { get; set; }
        public string Products { get; set; }
        public long CartId { get; set; }
        public long UserId { get; set; }
    }
}
