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
    public class AltitudeController : ControllerBase
    {
        private readonly ILogger<AltitudeController> _logger;
        private static List<Data.Altitude> _altitudes = new List<Data.Altitude>();

        public AltitudeController(ILogger<AltitudeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Data.Altitude> Get()
        {
            int numberOfValuesToGet = 5;
            return _altitudes.Skip(Math.Max(0, _altitudes.Count() - numberOfValuesToGet)).ToArray();
        }

        [HttpPost("{dt}")]
        public string Post(string dt)
        {
            Data.Altitude altitude = new Data.Altitude() { AltitudeM = double.Parse(dt) };
            _altitudes.Add(altitude);
            return "{\"response\":true}";
        }
    }
}
