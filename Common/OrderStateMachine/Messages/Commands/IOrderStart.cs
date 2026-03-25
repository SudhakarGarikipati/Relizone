namespace OrderStateMachine.Messages.Commands
{
    public interface IOrderStart
    {
        public Guid OrderId { get; }

        public string PaymentId { get; }

        public string Products { get; }

        public long CartId { get; }
    }
}
