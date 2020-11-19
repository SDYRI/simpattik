using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            List<RoundedColumnChartData> chartData = new List<RoundedColumnChartData>
            {
                  new RoundedColumnChartData { xValue= "Satu", yValue= 106, text= "Satu" },
                  new RoundedColumnChartData { xValue= "Dua", yValue= 103, text= "Dua" },
                  new RoundedColumnChartData { xValue= "Tiga", yValue= 198, text= "Tiga" },
                  new RoundedColumnChartData { xValue= "Empat", yValue= 189, text= "Empat" },
                  new RoundedColumnChartData { xValue= "Lima", yValue= 250, text= "Lima" }
            };
            ViewBag.dataSource = chartData;
            return View();
        }
        private class RoundedColumnChartData
        {
            public string xValue;
            public double yValue;
            public string text;
        }

        [Route("Privacy")]
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
