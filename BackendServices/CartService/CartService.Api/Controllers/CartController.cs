using CartService.Application.Dtos;
using CartService.Application.Services.Abstraction;
using CartService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : ControllerBase
    {
        readonly ICartAppService _cartAppService;

        public CartController(ICartAppService cartAppService)
        {
            _cartAppService = cartAppService;
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUserCart(long UserId)
        {
            var cart = _cartAppService.GetUserCart(UserId);
            //if (cart == null)
            //{
            //    return NotFound();
            //}
            return Ok(cart);
        }

       [HttpPost("{UserId}")]
       public async Task<IActionResult> AddItem(CartItem cartItemDto, long userId)
        {
            if (cartItemDto == null || cartItemDto.ItemId <= 0 || cartItemDto.Quantity <= 0)
            {
                return BadRequest("Invalid cart item data.");
            }
            var cart = _cartAppService.AddItem(userId, cartItemDto.CartId, cartItemDto.ItemId, cartItemDto.UnitPrice, cartItemDto.Quantity);
            return Ok(cart);
        }

        [HttpDelete("{CartId}/{CartItemId}")]
        public async Task<IActionResult> DeleteItem(long CartId, long CartItemId)
        {
            var result = _cartAppService.DeleteItem(CartId, CartItemId);
            if (result > 0)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("{CartId}")]
        public async Task<IActionResult> GetCart(long CartId)
        {
            var cart = _cartAppService.GetCart(CartId);
            return Ok(cart);
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetCartItemCount(long UserId)
        {
            var items = _cartAppService.GetCartItemCount(UserId);
            return Ok(items);
        }

        [HttpGet("{CartId}")]
        public IEnumerable<CartItemDto> GetItems(long CartId)
        {
            var cartItems = _cartAppService.GetCartItems(CartId);
            return cartItems;
        }

        [HttpGet("{CartId}")]
        public IActionResult MakeInActive(long CartId)
        {
            var result = _cartAppService.MakeInActive(CartId);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("{cartId}/{cartItemId}/{quantity}")]
        public IActionResult UpdateQuantity(long cartId, long cartItemId, int quantity)
        {
            if(quantity == 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            var count = _cartAppService.UpdateQuatity(cartId, cartItemId, quantity);
            if (count > 0)
            {
                return Ok(count);
            }
            return NotFound();
        }

    }
}
