using CartService.Domain.Entities;

namespace CartService.Application.Repositories
{
    public interface ICartReporitory
    {
        Cart GetUserCart(long userId);

        int GetCartItemCount(long userId);

        IEnumerable<CartItem> GetCartItems(long userId);

        Cart GetCart(long cartId);

        Cart AddItem(long cartId, long userId, CartItem cartItem);

        int DeleteItem(long cartId, long cartItemId);

        bool MakeInActive(long cartId);

        int UpdateQuatity(long cartId, long cartItemId, int quantity);

    }
}
