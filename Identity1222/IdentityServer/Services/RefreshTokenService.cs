using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        public Task<string> CreateRefreshTokenAsync(ClaimsPrincipal subject, Token accessToken, Client client)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken, Client client)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}
