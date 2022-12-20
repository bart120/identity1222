using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class ManageController : Controller
    {
        private readonly ConfigurationDbContext conf;
        public ManageController(ConfigurationDbContext conf, PersistedGrantDbContext pers)
        {
            this.conf = conf;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddClient()
        {
            Client client = new Client();
            client.ClientId = "client_console";
            client.ClientName = "Client en console";
            client.Enabled = true;
            client.RequirePkce = false;
            client.AllowedGrantTypes = GrantTypes.ClientCredentials;
            client.ClientSecrets = new List<Secret> { new Secret("secret_console".Sha256()) };

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

            var oid = new IdentityResources.OpenId();
            conf.IdentityResources.Add(oid.ToEntity());

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
    }
}
