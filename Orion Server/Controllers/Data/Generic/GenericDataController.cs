using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static Utilities.AsyncO _writer;
        internal static readonly List<T> _data = new();

        public GenericDataController(ILogger<U> logger, StreamWriter stream)
        {
            _logger = logger;
            _writer = new Utilities.AsyncO(stream);
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
        public async Task<string> Post([FromHeader] string authenticationKey, [FromBody] T requestData)
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
                    await _writer.WriteLine($"+ {JsonConvert.SerializeObject(requestData)}");
                    _data.Add(requestData);
                    returnVal = "{\"response\": true}";
                }
            }
            catch { }
            return returnVal;
        }
    }
}