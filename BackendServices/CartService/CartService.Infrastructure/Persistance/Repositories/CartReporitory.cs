using CartService.Application.Repositories;
using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Persistance.Repositories
{
    public class CartReporitory : ICartReporitory
    {
        readonly CartServiceDbContext _dbContext;
        public CartReporitory(CartServiceDbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            // Initialize the repository, e.g., with an in-memory store or database context
        }

        public Cart AddItem(long cartId, long userId, CartItem cartItem)
        {
            var cart = new Cart();
            if (cartId > 0)
            {
                cart = _dbContext.Carts.Find(cartId);
            }
            else
            {
                cart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId && c.IsActive);
            }
            if(cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(ci => ci.ItemId == cartItem.ItemId);
                if (item != null)
                {
                    item.Quantity += cartItem.Quantity; // Update quantity if item already exists
                }
                else
                {
                    cart.CartItems.Add(cartItem);
                }
                _dbContext.SaveChanges(); // Save changes to the database
            }
            else
            {
                cart = new Cart
                {
                    UserId = userId,
                    IsActive = true,
                    CartItems = [cartItem]
                };
                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges(); // Save changes to the database
            }
            return cart; // Return the updated or newly created cart
        }

        public int DeleteItem(long cartId, long cartItemId)
        {
            var cart = GetCart(cartId);
            if (cart == null)
            {
                return 0; // Cart not found
            }
            var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (itemToRemove == null)
            {
                return 0; // Cart item not found
            }
            _dbContext.CartItems.Remove(itemToRemove);
            return _dbContext.SaveChanges(); // Assuming SaveChanges() returns the number of affected rows
        }

        public Cart GetCart(long cartId)
        {
            return _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.Id == cartId && c.IsActive);
        }

        public int GetCartItemCount(long userId)
        {
           return _dbContext.Carts
                .Include(c => c.CartItems)
                .Where(c => c.UserId == userId)
                .SelectMany(c => c.CartItems)
                .Count();
        }

        public IEnumerable<CartItem> GetCartItems(long userId)
        {
            return _dbContext.Carts
                .Include(c => c.CartItems)
                .Where(c => c.UserId == userId)
                .SelectMany(c => c.CartItems)
                .ToList();
        }

        public Cart GetUserCart(long userId)
        {
            return _dbContext.Carts.Include(x => x.CartItems).Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
        }

        public bool MakeInActive(long cartId)
        {
           return _dbContext.Carts
                .Where(c => c.Id == cartId)
                .ExecuteUpdate(c => c.SetProperty(c => c.IsActive, false)) > 0;
        }

        public int UpdateQuatity(long cartId, long cartItemId, int quantity)
        {
           var cart = GetCart(cartId);
            if (cart == null)
            {
                return 0; // Cart not found
            }
            var itemToUpdate = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (itemToUpdate == null)
            {
                return 0; // Cart item not found
            }
            itemToUpdate.Quantity += quantity;
            _dbContext.CartItems.Update(itemToUpdate);
            return _dbContext.SaveChanges(); // Assuming SaveChanges() returns the number of affected rows
        }
    }
}
