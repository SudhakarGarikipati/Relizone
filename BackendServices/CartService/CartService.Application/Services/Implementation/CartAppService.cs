using AutoMapper;
using CartService.Application.Dtos;
using CartService.Application.HttpClients;
using CartService.Application.Repositories;
using CartService.Application.Services.Abstraction;
using CartService.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace CartService.Application.Services.Implementation
{
    public class CartAppService : ICartAppService
    {
        readonly ICartReporitory _cartReporitory;
        readonly IMapper _mapper;
        readonly IConfiguration _configuration;
        readonly CatalogServiceClient _catalogServiceClient;
        public CartAppService(ICartReporitory cartReporitory, IMapper mapper, IConfiguration configuration, CatalogServiceClient catalogServiceClient) {
            _cartReporitory = cartReporitory;
            _mapper = mapper;
            _configuration = configuration;
            _catalogServiceClient = catalogServiceClient;
        }

        private CartDto PopulateCartDetails(Cart cart)
        {
            try
            {
                var cartDto = _mapper.Map<CartDto>(cart);
                if (cartDto != null && cartDto.CartItems.Count > 0)
                {
                    var productId = cart.CartItems.Select(t => t.ItemId).ToArray();
                    var products = _catalogServiceClient.GetProductsByIds(productId).Result;
                    foreach (var item in cartDto.CartItems)
                    {
                        var product = products.FirstOrDefault(p => p.ProductId == item.ItemId);
                        if (product != null)
                        {
                            item.Name = product.Name;
                            item.ImageUrl = product.ImageUrl;
                        }
                    }

                    foreach (var item in cartDto.CartItems)
                    {
                        cartDto.Total += item.UnitPrice * item.Quantity;
                    }
                    cartDto.Tax = cartDto.Total * Convert.ToDecimal(_configuration["Tax"]) / 100;
                    cartDto.GrandTotal = cartDto.Total + cartDto.Tax;
                }
                return cartDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CartDto AddItem(long userId, long cartId, int itemId, decimal unitPrice, int quantity)
        {
            var cartItem = new CartItem
            {
                CartId = cartId,
                ItemId = itemId,
                UnitPrice = unitPrice,
                Quantity = quantity
            };
            var cart = _cartReporitory.AddItem(cartId, userId, cartItem);
            return _mapper.Map<CartDto>(cart);
        }

        public int DeleteItem(long cartId, long cartItemId)
        {
            return _cartReporitory.DeleteItem(cartId, cartItemId);
        }

        public CartDto GetCart(long cartId)
        {
            var cart = _cartReporitory.GetCart(cartId);
            if(cart == null)
            {
                return _mapper.Map<CartDto>(null);
            }
            return _mapper.Map<CartDto>(cart);
        }

        public int GetCartItemCount(long userId)
        {
            if(userId <= 0)
            {
                return 0;
            }
            return _cartReporitory.GetCartItemCount(userId);
        }

        public IEnumerable<CartItemDto> GetCartItems(long userId)
        {
            var cartItems = _cartReporitory.GetCartItems(userId);
            if (cartItems == null || !cartItems.Any())
            {
                return _mapper.Map<IEnumerable<CartItemDto>>(null);
            }
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public CartDto GetUserCart(long userId)
        {
            var cart = _cartReporitory.GetUserCart(userId);
            if(cart == null)
            {
                return _mapper.Map<CartDto>(null);
            }
            var cartModel = PopulateCartDetails(cart);
            return cartModel;
        }

        public bool MakeInActive(long cartId)
        {
            return _cartReporitory.MakeInActive(cartId);
        }

        public int UpdateQuatity(long cartId, long cartItemId, int quantity)
        {
            return _cartReporitory.UpdateQuatity(cartId, cartItemId, quantity);
        }
    }
}
