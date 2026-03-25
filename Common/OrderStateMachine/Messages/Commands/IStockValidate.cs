namespace OrderStateMachine.Messages.Commands
{
    public interface IStockValidate
    {
        public Guid OrderId { get; }

        public string PaymentId { get; }

        public string Products { get; }

        public long CartId { get; }
    }
}
