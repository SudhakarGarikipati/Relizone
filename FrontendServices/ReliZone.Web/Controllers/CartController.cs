using Microsoft.AspNetCore.Mvc;
using ReliZone.Web.Helpers;
using ReliZone.Web.HttpClients;
using ReliZone.Web.Models;

namespace ReliZone.Web.Controllers
{
    public class CartController : BaseController
    {
        readonly CartServiceClient _cartServiceClient;

        public CartController(CartServiceClient cartServiceClient)
        {
            _cartServiceClient = cartServiceClient;
        }

        public IActionResult Checkout()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(AddressModel addressModel)
        {
            if (ModelState.IsValid)
            {
                TempData.Set("ShippingAddress", addressModel);
                return RedirectToAction("Index","Payment");
            }
            return View();
        }

        public IActionResult Index()
        {
            if (CurrentUser != null)
            {
                var cartModel = _cartServiceClient.GetUserCartAsync(CurrentUser.UserId).Result;
                return View(cartModel);
            }
            return RedirectToAction("Login", "Account", new { returnUrl = "/" });
        }

        [Route("Cart/AddToCart/{itemId}/{unitprice}/{quantity}")]
        public async Task<IActionResult> AddToCart(int itemId,decimal unitprice, int quantity)
        {
            if (quantity <= 0)
            {
                // Handle invalid quantity (e.g., show a message to the user)
                return View("Error", "Quantity must be greater than zero.");
            }
            var carItemModel = new CartItemModel
            {
                ItemId = itemId,
                UnitPrice = unitprice,
                Quantity = quantity
            };

            var cartModel = await _cartServiceClient.AddItemAsync(carItemModel, CurrentUser.UserId);
            if (cartModel != null)
            {
                return Json(new {status= "success", count = cartModel.CartItems.Count});
            }
            else
            {
                // Handle error (e.g., show a message to the user)
                return Json(new { status = "Failes", count = 0 });
            }
        }

        [Route("Cart/UpdateQuantity/{cartId}/{cartItemId}/{quantity}")]
        public IActionResult UpdateQuantity(long cartId, long cartItemId, int quantity)
        {
            var count = _cartServiceClient.UpdateQuantityAsync(cartId, cartItemId, quantity).Result;
            return Json(count);
        }

        [Route("Cart/DeleteItem/{cartId}/{cartitemid}")]
        public IActionResult DeleteItem(long cartId, long cartitemid)
        {
            var count = _cartServiceClient.DeleteItem(cartId, cartitemid).Result;
            return Json(count);
        }

        public IActionResult GetCartCount()
        {
            if (CurrentUser != null)
            {
                    var count = _cartServiceClient.GetCartItemCountAsync(CurrentUser.UserId).Result;
                    return Json(count);
            }
            return Json(0);
        }
    }
}
