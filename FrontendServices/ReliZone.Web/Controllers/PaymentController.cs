using Microsoft.AspNetCore.Mvc;
using ReliZone.Web.Helpers;
using ReliZone.Web.HttpClients;
using ReliZone.Web.Messages;
using ReliZone.Web.Models;

namespace ReliZone.Web.Controllers
{
    public class PaymentController : BaseController
    {
        readonly CartServiceClient _cartServiceClient;
        readonly PaymentServiceClient _paymentServiceClient;
        readonly IConfiguration _configuration;
        readonly OrderServiceClient _orderServiceClient;

        public PaymentController(CartServiceClient cartServiceClient, PaymentServiceClient paymentServiceClient, IConfiguration configuration, OrderServiceClient orderServiceClient)
        {
            _cartServiceClient = cartServiceClient;
            _paymentServiceClient = paymentServiceClient;
            _configuration = configuration;
            _orderServiceClient = orderServiceClient;
        }

        public IActionResult Index()
        {
            if(CurrentUser == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Payment" });
            }

            var cartModel = _cartServiceClient.GetUserCartAsync(CurrentUser.UserId).Result;
            if (cartModel != null) {
                var paymentModel = new PaymentModel
                {
                    Cart = cartModel,
                    Currency = "INR",
                    Description = string.Join(",", cartModel.CartItems.Select(c => c.Name)),
                    GrandTotal = cartModel.GrandTotal,
                    RazorpayKey = _configuration["RazorPay:Key"],
                   
                };
                RazorPayOrderModel razorpayOrder = new RazorPayOrderModel
                {
                    Amount = Convert.ToInt32(paymentModel.GrandTotal * 100),
                    Currency = paymentModel.Currency,
                    Receipt = Guid.NewGuid().ToString()
                };
                paymentModel.OrderId = _paymentServiceClient.CreateOrderAsync(razorpayOrder).Result;
                return View(paymentModel);
            }
            // You can pass the cartModel to the view if needed
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Status(IFormCollection form) {
            if (!string.IsNullOrEmpty(form["#rzp_paymentid"]))
            {
                string paymentId = form["#rzp_paymentid"];
                string orderId = form["#rzp_orderid"];
                string signature = form["#rzp_signature"];
                string transactionId = form["Receipt"];
                string currancy = form["Currancy"];

                PaymentConfirmModel paymentConfirmModel = new PaymentConfirmModel
                {
                    PaymentId = paymentId,
                    OrderId = orderId,
                    Signature = signature
                };

                var status = _paymentServiceClient.VerifyPaymentAsync(paymentConfirmModel).Result;
                if(status is "Captured" or "Completed")
                {
                    CartModel cartModel = _cartServiceClient.GetUserCartAsync(CurrentUser.UserId).Result;
                    TransactionModel model = new TransactionModel();
                    model.CartId = cartModel.Id;
                    model.Total = cartModel.Total;
                    model.Tax = cartModel.Tax;
                    model.GrandTotal = cartModel.GrandTotal;
                    model.CreatedDate = cartModel.CreatedDate;

                    model.Status = status;
                    model.TransactionId = transactionId;
                    model.Currency = currancy;
                    model.Email = CurrentUser.Email;
                    model.UserId = CurrentUser.UserId;
                    model.Id = paymentId;

                    bool result = _paymentServiceClient.SavePaymentDetailsAsync(model).Result;
                    if (result)
                    {
                        OrderMessage orderMessage = new OrderMessage
                        {
                            CartId = cartModel.Id,
                            UserId = CurrentUser.UserId,
                            PaymentId = paymentId,
                            Products =string.Join(",",cartModel.CartItems.Select(c =>$"{c.ItemId}:" +
                            $"{c.Quantity}"))
                        };
                        _orderServiceClient.CreateOrder(orderMessage).Wait();
                        _cartServiceClient.MakeInActiveAsync(cartModel.Id).Wait();
                        TempData.Set("Receipt", model);
                        return RedirectToAction("Receipt");
                    }
                }
            }
            ViewBag.Message = "Payment failed. Please try again.";
            return View(); 
        } 

        public IActionResult Receipt()
        {
            TransactionModel model = TempData.Get<TransactionModel>("Receipt");
            return View(model);
        }
    }
}
