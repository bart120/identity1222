using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVCClientCred.Service
{
    public class ApiTokenCacheClientService
    {
        private const int cacheExpirationDays = 1;
        private readonly HttpClient _http;
        private static readonly object _lock = new object();
        private IDistributedCache _cache;

        public ApiTokenCacheClientService(IDistributedCache cache, HttpClient client)
        {
            _cache = cache;
            _http = client;
        }

        private async Task<AccessToken> getToken(string clientId, string apiScope, string secret)
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(_http, new ClientCredentialsTokenRequest
            {
                Scope = apiScope,
                ClientSecret = secret,
                ClientId = clientId,
                Address = @"https://localhost:44361/connect/token"
            });
            if (tokenResponse.IsError)
            {
                throw new ApplicationException($"Stats code: {tokenResponse.HttpStatusCode}, Error: {tokenResponse.Error}");
            }

            return new AccessToken
            {
                ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                Token = tokenResponse.AccessToken
            };
        }

        private void addToCache(string key, AccessToken accessToken)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(cacheExpirationDays));
            lock (_lock)
            {
                _cache.SetString(key, JsonSerializer.Serialize(accessToken));
            }
        }

        private AccessToken GetFromCache(string key)
        {
            lock (_lock)
            {
                var item = _cache.GetString(key);
                if (item != null)
                {
                    return JsonSerializer.Deserialize<AccessToken>(item, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            return null;
        }

        public async Task<string> GetApiTokenAsync(string clientId, string apiScope, string secret)
        {
            var accessToken = GetFromCache($"{clientId}-{apiScope}");
            if(accessToken != null)
            {
                if(accessToken.ExpiresIn > DateTime.UtcNow)
                {
                    return accessToken.Token;
                }
            }

            var newAccessToken = await getToken(clientId, apiScope, secret);
            addToCache($"{clientId}-{apiScope}", newAccessToken);
            return newAccessToken.Token;
        }


    }

    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
