using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace OrionServer.Controllers
{
    public class AdminRequest
    {
        public string authenticationID { get; set; }
        public string requestID { get; set; }
        public string requestData { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AdminAPIController : ControllerBase
    {
        private readonly ILogger<AdminAPIController> _logger;
        private static Utilities.AsyncO _writer;

        private Dictionary<string, Func<string, string>> response = new Dictionary<string, Func<string, string>>()
        {
            { "getAllKeys", (string param) => { return JsonConvert.SerializeObject(Utilities.Authenticator.GetAllKeys()); }},
            { "addKey", (string param) => { Utilities.Authenticator.AuthorizeKey(param); return $"{{\"response\": true }}"; } }
        };

        public AdminAPIController(ILogger<AdminAPIController> logger)
        {
            _logger = logger;
            _writer = new Utilities.AsyncO(new StreamWriter($"{Constants.DataFolder}/admin.dat"));
        }

        [HttpPost]
        public async Task<string> Post([FromBody] AdminRequest requestData)
        {
            string returnVal = "{\"response\": false}";
            try
            {
                if (!Utilities.Authenticator.IsAuthorizedKey(requestData.authenticationID))
                {
                    returnVal = "{\"response\": false, \"responseMessage\": \"Unauthorized User\"}";
                }
                else
                {
                    await _writer.WriteLine($"+ {JsonConvert.SerializeObject(requestData)}");
                    returnVal = response[requestData.requestID](requestData.requestData);
                }
            }
            catch { }
            return returnVal;
        }
    }
}