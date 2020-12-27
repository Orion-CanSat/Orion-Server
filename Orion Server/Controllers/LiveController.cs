using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrionServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OrionServer.Controllers
{
    public class LiveController : Controller
    {
        private readonly ILogger<LiveController> _logger;

        public LiveController(ILogger<LiveController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string id)
        {
            string[] charts = id.Split('-');
            List<string[]> chartsToShow = new List<string[]>();
            for (int i = 0; i < charts.Length; i++)
                chartsToShow.Add(charts[i].Split('_'));

            ViewBag.ChartsToShow = chartsToShow;

            return View(new LiveViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
