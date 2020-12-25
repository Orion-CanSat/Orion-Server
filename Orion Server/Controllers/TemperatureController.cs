using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemperatureController : ControllerBase
    {
        private readonly ILogger<TemperatureController> _logger;
        private static List<Data.Temperature> _temperatures = new List<Data.Temperature>();

        public TemperatureController(ILogger<TemperatureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Data.Temperature> Get()
        {
            int numberOfValuesToGet = 5;
            return _temperatures.Skip(Math.Max(0, _temperatures.Count() - numberOfValuesToGet)).ToArray();
        }

        [HttpPost("{dt}")]
        public string Post(string dt)
        {
            Data.Temperature temperature = new Data.Temperature() { TemperatureC = double.Parse(dt) };
            _temperatures.Add(temperature);
            return "{\"response\":true}";
        }
    }
}
