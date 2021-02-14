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
        public async Task<Utilities.Response> Post([FromBody] Utilities.Request request)
        {
            Utilities.Response returnVal = new Utilities.Response();
            try
            {
                if (!Utilities.Authenticator.IsAuthorizedKey(request.AuthenticationID))
                {
                    returnVal.Error = true;
                    returnVal.ErrorMessage = "Unauthorized User";
                }
                else
                {
                    await _writer.WriteLine($"+ {JsonConvert.SerializeObject(request.RequestData)}");
                    Console.WriteLine(request.RequestData);
                    T temp = JsonConvert.DeserializeObject<T>(request.RequestData);
                    await Program.sqlConnection.Insert<T>(temp);
                    _data.Add(temp);

                    returnVal.Error = false;
                    returnVal.ResponseData = "true";
                }
            }
            catch
            {
                returnVal.Error = true;
                returnVal.ErrorMessage = "Unexpected error";
            }
            
            return returnVal;
        }
    }
}