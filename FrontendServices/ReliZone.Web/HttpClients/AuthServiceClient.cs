using ReliZone.Web.Models;
using System.Text;
using System.Text.Json;

namespace ReliZone.Web.HttpClients
{
    public class AuthServiceClient
    {
       readonly HttpClient _httpClient;

        public AuthServiceClient(HttpClient httpClient) { 
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<UserModel> LoginAsync(LoginModel loginModel)
        {
            try
            {
                StringContent content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responceContent = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<UserModel>(responceContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    throw new Exception("Login failed: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> Register(SignUpModel signUpModel)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(signUpModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/SignUp", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception("Registration failed: " + response.ReasonPhrase);
            }
        }

    }
}
