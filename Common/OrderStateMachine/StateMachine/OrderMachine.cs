using MassTransit;
using OrderStateMachine.Database.Entities;
using OrderStateMachine.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.StateMachine
{
    public class OrderMachine : MassTransitStateMachine<OrderState>
    {
        public OrderMachine()
        { 
            Event(() => OrderStarted, x =>x.CorrelateById(context => context.Message.OrderId));
            Event(() => StockValidated, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => OrderCancelled, x => x.CorrelateById(context => context.Message.OrderId));  
            Event(() => OrderAccepted, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => PaymentCancelled, x => x.CorrelateById(context => context.Message.OrderId));

            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderStarted)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.PaymentId = context.Message.PaymentId;
                    context.Saga.CartId = context.Message.CartId;
                    context.Saga.Products = context.Message.Products;
                })
            .TransitionTo(Started)
                .Publish(context => new StockValidate(context.Saga)));
            
            During(Started,
                When(OrderCancelled)
                .Then(context =>
                {
                    context.Saga.OrderCancelDateTime = DateTime.Now;
                })
                .TransitionTo(Cancelled)); // save to db
             
            During(Started, 
                When(OrderAccepted)
                .Then(context =>
                {
                    context.Saga.OrderAcceptDateTime = DateTime.Now;
                })
                .TransitionTo(Accepted));  // save to db

        }

        public State Started;
        public State Validated;
        public State Accepted;
        public State Cancelled;

        public Event<IOrderStarted> OrderStarted { get; set; }

        public Event<IOrderCancelled> OrderCancelled { get; set; }

        public Event<IOrderAccepted> OrderAccepted { get; set; }

        public Event<IPaymentCancelled> PaymentCancelled { get; set; }

        public Event<IStockValidated> StockValidated { get; set; }

    }
}
