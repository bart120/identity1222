using IdentityModel;
using IdentityServer.AspIdentity;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //if (context.Caller == "UserInfoPoint")
            //{
            var user = await _userManager.GetUserAsync(context.Subject);
            var roles = await _userManager.GetRolesAsync(user);
            var tokenProfileClaims = new List<Claim>();
            foreach (var item in roles)
            {
                tokenProfileClaims.Add(new Claim(JwtClaimTypes.Role, item));
            }

            if(context.RequestedResources.RawScopeValues.Any(x => x == "api_demo_scope_delete"))
            {
                var claims = (await _userManager.GetClaimsAsync(user)).Where(x => x.Type == "weather");
                context.IssuedClaims.AddRange(claims);
            }

            context.IssuedClaims.Add(new Claim("Firstname", user.Firstname));
            context.IssuedClaims.AddRange(tokenProfileClaims);
            //}

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            /*if (context.Caller == "UserInfoPoint")
            {
               
            }
            context.IsActive = false;*/
        }
    }
}
