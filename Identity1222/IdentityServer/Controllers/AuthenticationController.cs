using IdentityServer.AspIdentity;
using IdentityServer.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;

        public AuthenticationController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = ReturnUrl };
            return View(model);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
                ModelState.AddModelError("Email", "Login/mot de passe invalide.");
            }
            return View(model);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            if(User?.Identity.IsAuthenticated == true)
            {
                var contextLogout = await _interaction.GetLogoutContextAsync(model.LogoutId);

                await _signInManager.SignOutAsync();

                return Redirect(contextLogout.PostLogoutRedirectUri);
            }
            return BadRequest();
        }
    }
}
