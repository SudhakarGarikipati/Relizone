using ReliZone.Web.Models;
using System.Text;
using System.Text.Json;

namespace ReliZone.Web.HttpClients
{
    public class CartServiceClient
    {
        readonly HttpClient _httpClient;

        readonly JsonSerializerOptions jsonSerializerOptions;

        public CartServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            jsonSerializerOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<CartModel> GetUserCartAsync(long userId)
        {
            var response = await _httpClient.GetAsync("cart/getusercart/" + userId);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if(!string.IsNullOrEmpty(responseContent))
                {
                    return JsonSerializer.Deserialize<CartModel>(responseContent, jsonSerializerOptions);
                }
                return null;
            }
            else
            {
                throw new Exception($"Error fetching user cart: {response.ReasonPhrase}");
            }
        }

        public async Task<CartModel> GetCartAsync(long cartId)
        {
            var response = await _httpClient.GetAsync("cart/GetCart/" + cartId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cartModel = JsonSerializer.Deserialize<CartModel>(content, jsonSerializerOptions);
                return cartModel;
            }
            else
            {
                throw new Exception($"Error fetching cart: {response.ReasonPhrase}");
            }
        }

        public async Task<CartModel> AddItemAsync(CartItemModel cartItem, long userId)
        {
            if (cartItem == null || cartItem.ItemId <= 0 || cartItem.Quantity <= 0)
            {
                throw new ArgumentException("Invalid cart item data.");
            }
            var content = new StringContent(JsonSerializer.Serialize(cartItem), Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync("cart/AddItem/" + userId, content);
            if (response.Result.IsSuccessStatusCode)
            {
                var resultContent = await response.Result.Content.ReadAsStringAsync();
                var cartModel = JsonSerializer.Deserialize<CartModel>(resultContent, jsonSerializerOptions);
                return cartModel;
            }
            else
            {
                throw new Exception($"Error adding item to cart: {response.Result.ReasonPhrase}");
            }
        }

        public async Task<int> DeleteItem(long cartId, long cartitemid)
        {
            var response = await _httpClient.DeleteAsync($"cart/DeleteItem/{cartId}/{cartitemid}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return int.Parse(content);
            }
            else
            {
                throw new Exception($"Error deleting item from cart: {response.ReasonPhrase}");
            }
        }

        public async Task<int> GetCartItemCountAsync(long userId)
        {
            var response = await _httpClient.GetAsync("cart/GetCartItemCount/" + userId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return int.Parse(content);
            }
            else
            {
                throw new Exception($"Error fetching cart item count: {response.ReasonPhrase}");
            }
        }

        public async Task<IEnumerable<CartItemModel>> GetCartItemsAsync(long cartId)
        {
            var response = await _httpClient.GetAsync("cart/GetItems/" + cartId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cartItems = JsonSerializer.Deserialize<IEnumerable<CartItemModel>>(content, jsonSerializerOptions);
                return cartItems;
            }
            else
            {
                throw new Exception($"Error fetching cart items: {response.ReasonPhrase}");
            }
        }

        public async Task<bool> MakeInActiveAsync(long cartId)
        {
            var response = await _httpClient.GetAsync("cart/MakeInActive/" + cartId);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error making cart inactive: {response.ReasonPhrase}");
            }
        }

        public async Task<int> UpdateQuantityAsync(long cartId, long cartItemId, int quantity)
        {
            if (quantity == 0)
            {
                throw new ArgumentException("Quantity must be non zero.");
            }
            var response = await _httpClient.GetAsync($"cart/UpdateQuantity/{cartId}/{cartItemId}/{quantity}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return int.Parse(content);
            }
            else
            {
                throw new Exception($"Error updating cart item quantity: {response.ReasonPhrase}");
            }
        }


    }
}
