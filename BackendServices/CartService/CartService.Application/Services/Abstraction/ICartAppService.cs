using CartService.Application.Dtos;

namespace CartService.Application.Services.Abstraction
{
    public interface ICartAppService
    {

        CartDto GetUserCart(long userId);

        int GetCartItemCount(long userId);

        IEnumerable<CartItemDto> GetCartItems(long userId);

        CartDto GetCart(long cartId);

        CartDto AddItem(long userId, long cartId, int itemId, decimal unitPrice, int quantity);

        int DeleteItem(long cartId, long cartItemId);

        bool MakeInActive(long cartId);

        int UpdateQuatity(long cartId, long cartItemId, int quantity);
    }
}
