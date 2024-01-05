using Blazored.LocalStorage;
using Hybrid.Service.Shared.Data;
using Hybrid.Service.Shared.Interface;
using Newtonsoft.Json.Linq;
using Supabase.Gotrue;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hybrid.Web.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public UserService(IHttpClientFactory httpClientFactory,
            ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserInfo[]?> GeUsersAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("HybridServer");
            var token = await _localStorage.GetItemAsStringAsync("token");

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var httpResponseMessage = await httpClient.GetAsync(
            "User/GetUsers");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //String content = await httpResponseMessage.Content.ReadAsStringAsync();
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                UserInfo[]? users = await JsonSerializer.DeserializeAsync
                   <UserInfo[]?>(contentStream);

                return users;
            }
            else
            {
                return null;
            }
        }

        public Task UpdateRolesAsync(UserInfo userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
