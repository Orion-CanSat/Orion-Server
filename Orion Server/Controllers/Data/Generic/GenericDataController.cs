using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrionServer.Controllers.Data.Generic
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericDataController<T, U> : ControllerBase where T: class where U: class
    {
        private readonly ILogger<U> _logger;
        private static readonly List<T> _data = new();

        public GenericDataController(ILogger<U> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<T> Get()
        {
            int numberOfValuesToGet = 5;
            return _data.Skip(Math.Max(0, _data.Count() - numberOfValuesToGet)).ToArray();
        }

        [HttpPost("{dt}")]
        public string Post(string dt)
        {
            T temp = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(dt);
            _data.Add(temp);
            return "{\"response\":true}";
        }
    }
}