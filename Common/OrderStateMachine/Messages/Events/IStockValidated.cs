namespace OrderStateMachine.Messages.Events
{
    public interface IStockValidated
    {
        public Guid OrderId { get; }
        public string PaymentId { get; }

        public string Products { get; }

        public string CartId { get; }
    }
}
