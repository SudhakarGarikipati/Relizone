namespace OrderStateMachine.Messages.Events
{
    public interface IOrderStarted
    {
        Guid OrderId { get; }
        string PaymentId { get; }
        string Products { get; }
        long CartId { get; }
    }
}
