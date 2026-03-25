namespace OrderStateMachine.Messages.Events
{
    public interface IPaymentCancelled
    {
        Guid OrderId { get; }
        string PaymentId { get; }

        long CartId { get; }

        string Products { get; }
    }
}
