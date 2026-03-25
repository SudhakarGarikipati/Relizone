using OrderStateMachine.Database.Entities;
using OrderStateMachine.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.StateMachine
{
    public class StockValidate : IStockValidate
    {
        private readonly OrderState _orderState;

        public StockValidate(OrderState orderState)
        {
            _orderState = orderState;
        }

        public Guid OrderId => _orderState.OrderId;

        public string PaymentId => _orderState.PaymentId;

        public string Products => _orderState.Products;

        public long CartId => _orderState.CartId;
    }
}
