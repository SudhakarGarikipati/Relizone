namespace OrderStateMachine.Messages.Events
{
    public interface IOrderAccepted
    {
        Guid OrderId { get; }
        string PaymentId { get; }
        string Products { get; }
        long CartId { get; }

        public DateTime AcceptedDateTime { get; }
    }
}
