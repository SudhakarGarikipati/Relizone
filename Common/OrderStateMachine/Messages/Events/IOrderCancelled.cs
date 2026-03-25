namespace OrderStateMachine.Messages.Events
{
    public interface IOrderCancelled
    {
        Guid OrderId { get; }
        string PaymentId { get; }
        long CartId { get; }
    }
}
