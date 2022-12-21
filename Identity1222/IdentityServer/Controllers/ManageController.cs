using IdentityServer.AspIdentity;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class ManageController : Controller
    {
        private readonly ConfigurationDbContext conf;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ManageController(ConfigurationDbContext conf, PersistedGrantDbContext pers, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.conf = conf;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddClient()
        {
            /*Client client = new Client();
            client.ClientId = "client_console";
            client.ClientName = "Client en console";
            client.Enabled = true;
            client.RequirePkce = false;
            client.AllowedGrantTypes = GrantTypes.ClientCredentials;
            client.ClientSecrets = new List<Secret> { new Secret("secret_console".Sha256()) };*/

            Client client = new Client();
            client.ClientId = "client_mvc";
            client.ClientName = "Client en MVC";
            client.Enabled = true;
            client.RequirePkce = false;
            client.AllowedGrantTypes = GrantTypes.Code;
            client.ClientSecrets = new List<Secret> { new Secret("secret_mvc".Sha256()) };
            client.AllowedScopes = new List<string> { "openid", "api_demo_scope" };

            conf.Clients.Add(client.ToEntity());
            await conf.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> AddApiScope()
        {
            /* ApiScope scope = new ApiScope();
             scope.Name = "api_demo_scope";
             scope.Enabled = true;
             conf.ApiScopes.Add(scope.ToEntity());*/

            //var oid = new IdentityResources.OpenId();
            //conf.IdentityResources.Add(oid.ToEntity());
            /*var scopeProfile = new IdentityResources.Profile();
            conf.IdentityResources.Add(scopeProfile.ToEntity());
            var scopeEmail = new IdentityResources.Email();
            conf.IdentityResources.Add(scopeEmail.ToEntity());*/

            ApiScope scope = new ApiScope();
            scope.Name = "api_demo_scope_read";
            scope.Enabled = true;
            scope.UserClaims.Add("read");
            conf.ApiScopes.Add(scope.ToEntity());

            scope = new ApiScope();
            scope.Name = "api_demo_scope_delete";
            scope.Enabled = true;
            scope.UserClaims.Add("delete");
            conf.ApiScopes.Add(scope.ToEntity());

            await conf.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> AddApi()
        {
            ApiResource api = new ApiResource();
            api.Name = "api_demo";
            api.Enabled = true;
            conf.ApiResources.Add(api.ToEntity());

            await conf.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> AddUser()
        {
            var role = new IdentityRole { Name = "UTILISATEUR", NormalizedName = "UTILISATEUR" };
            await _roleManager.CreateAsync(role);
            role = new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" };
            await _roleManager.CreateAsync(role);


            var user1 = new User { Email = "bob@gmail.com", UserName = "bob@gmail.com", Lastname = "Leponge", Firstname = "Bob" };
            var user2 = new User { Email = "toto@gmail.com", UserName = "toto@gmail.com", Lastname = "Toto", Firstname = "Toto" };

            var res = await _userManager.CreateAsync(user1, "Toto007$");
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(user1, "UTILISATEUR");
            }

            res = await _userManager.CreateAsync(user2, "Toto007$");
            if (res.Succeeded)
            {
                await _userManager.AddToRolesAsync(user2, new List<string> { "UTILISATEUR", "ADMIN" });
            }

            return Ok();
        }
    }
}
