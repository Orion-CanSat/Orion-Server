using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace OrionServer.Controllers.Data.Generic
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericDataController<T, U> : ControllerBase where T: class where U: class
    {
        private readonly ILogger<U> _logger;
        internal static readonly List<T> _data = new();

        public GenericDataController(ILogger<U> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            string returnVal = "{\"response\": false}";
            try
            {
                returnVal = $"{{\"response\": true, \"responseData\": {JsonConvert.SerializeObject(_data[_data.Count - 1])}}}";
            }
            catch { }

            return returnVal;
        }

        [HttpPost]
        public string Post([FromHeader] string authenticationKey, [FromBody] T requestData)
        {
            string returnVal = "{\"response\": false}";
            try
            {
                if (!Utilities.Authenticator.IsAuthorizedKey(authenticationKey))
                {
                    returnVal = "{\"response\": false, \"responseMessage\": \"Unauthorized User\"}";
                }
                else
                {
                    _data.Add(requestData);
                    returnVal = "{\"response\": true}";
                }
            }
            catch { }
            return returnVal;
        }
    }
}