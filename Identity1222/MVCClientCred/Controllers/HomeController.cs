using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClientCred.Models;
using MVCClientCred.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClientCred.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherService _serv;

        public HomeController(ILogger<HomeController> logger, IWeatherService serv)
        {
            _serv = serv;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var weathers = await _serv.GetWeathers();
            return View(weathers);
        }

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
