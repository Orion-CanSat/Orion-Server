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
            try
            {
                List<List<List<Utilities.Pair<string, string>>>> chartsToShow = new();

                string[] rows = id.Split('-');
                foreach (string row in rows)
                {
                    string[] individualCharts = row.Split('.');
                    List<List<Utilities.Pair<string, string>>> chartsThisLine = new();
                    foreach (string individualChart in individualCharts)
                    {
                        List<Utilities.Pair<string, string>> chartElements = new();

                        foreach (string chartGraph in individualChart.Split('_'))
                        {
                            Utilities.Pair<string, string> chartElement = new(chartGraph.Split('~')[0], chartGraph.Split('~')[1]);

                            chartElements.Add(chartElement);
                        }

                        chartsThisLine.Add(chartElements);
                    }
                    chartsToShow.Add(chartsThisLine);
                }

                ViewBag.ChartsToShow = chartsToShow;

                return View(new LiveViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch
            {
                return Redirect("/Home/Error");
            }
        }
    }
}
