using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Models;
using MVCClient.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    /*[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TestAttribute : Attribute
    {
        public TestAttribute()
        {

        }
    }*/

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherService _weatherService;


        public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        //[Test]
        public async Task<IActionResult> Index()
        {
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var idtoken = await HttpContext.GetTokenAsync("id_token");
            var refresToken = await HttpContext.GetTokenAsync("refresh_token");
            var u = User;

            var auth = await HttpContext.AuthenticateAsync();
            var weathers = await _weatherService.GetWeathers();
            return View(weathers);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
