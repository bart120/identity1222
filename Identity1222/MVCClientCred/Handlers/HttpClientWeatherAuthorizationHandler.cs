using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MVCClientCred.Service;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MVCClientCred.Handlers
{
    public class HttpClientWeatherAuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiTokenCacheClientService _apiTokenCache;

        public HttpClientWeatherAuthorizationHandler(/*IHttpContextAccessor _httpContextAccessor, */ApiTokenCacheClientService apiTokenCache)
        {
            _apiTokenCache = apiTokenCache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            //request.Headers.Add("Authorization", new List<string> { authorizationHeader });
            //var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            var token = await _apiTokenCache.GetApiTokenAsync("client_console", "api_demo_scope", "secret_console");
            request.Headers.Add("Authorization", $"Bearer {token}");
            return await base.SendAsync(request, cancellationToken);
        }


    }
}
