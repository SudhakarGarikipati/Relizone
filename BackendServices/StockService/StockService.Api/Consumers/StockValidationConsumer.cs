using MassTransit;
using OrderStateMachine.Messages.Commands;
using OrderStateMachine.Messages.Events;
using StockService.Application.Services.Abstraction;

namespace StockService.Api.Consumers
{
    public class StockValidationConsumer : IConsumer<IStockValidate>
    {
        IStockAppService _stockAppService;

        public StockValidationConsumer(IStockAppService stockAppService)
        {
            _stockAppService = stockAppService;
        }

        public async Task Consume(ConsumeContext<IStockValidate> context)
        {
            try
            {
                var data = context.Message;
                if (data != null)
                {
                    var products = data.Products.Split(',');
                    var isStockAvailable = true;
                    foreach (var product in products)
                    {
                        var prodDetails = product.Split(':');
                        int productId = Convert.ToInt32(prodDetails[0]);
                        int quantity = Convert.ToInt32(prodDetails[1]);
                        // Check stock availability and reserve stock logic here
                        isStockAvailable = _stockAppService.CheckStockAvailibility(productId, quantity);
                        if(!isStockAvailable)
                        {
                            break;
                        }
                    }
                    if(!isStockAvailable)
                    {
                        //Cancel the order
                        await context.Publish<IOrderCancelled>(new {
                            OrderId = data.OrderId, 
                            PaymentId= data.PaymentId,
                            CartId = data.CartId
                        });
                    }
                    else
                    {
                        //reserve stock
                        foreach (var product in products)
                        {
                            var prodDetails = product.Split(':');
                            int productId = Convert.ToInt32(prodDetails[0]);
                            int quantity = Convert.ToInt32(prodDetails[1]);
                            _stockAppService.ReserveStock(productId, quantity);
                        }
                        //goto next step
                        await context.Publish<IOrderAccepted>(new {
                            OrderId = data.OrderId, 
                            PaymentId= data.PaymentId,
                            CartId = data.CartId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
