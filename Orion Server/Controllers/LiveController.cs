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

        public IActionResult Temperature()
        {
            return View(new LiveViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
