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

    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public RedirectResult Index([FromQuery] string id)
        {
            if ((id == "") || (!Utilities.Authenticator.IsAuthorizedKey(id)))
                return Redirect("/Admin/LogIn");
            else
                return Redirect($"/Admin/Admin?id={id}");
        }

        public IActionResult LogIn()
        {
            return View(new LogInViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Admin([FromQuery] string id)
        {
            if ((id == "") || (!Utilities.Authenticator.IsAuthorizedKey(id)))
                return View("LogIn", new LogInViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            else
            {
                ViewBag.Id = id;
                return View(new AdminViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, AuthenticationKey = id });
            }
        }
    }
}