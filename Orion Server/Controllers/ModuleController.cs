#nullable enable

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrionServer.Utilities;

namespace OrionServer.Controllers
{
    public class Request
    {
        public string authenticationID { get; set; }
        public string requestID { get; set; }
        public string requestData { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ModuleAPIController : ControllerBase
    {
        private readonly ILogger<ModuleAPIController> _logger;
        public static Dictionary<string, Utilities.Pair<bool, Utilities.AssemblyLoader>> _loadedAssemblies = new();
        private static Utilities.AsyncO? _writer = null;

        private Dictionary<string, Func<string, bool, string>> response = new Dictionary<string, Func<string, bool, string>>()
        {
            { "getAllActiveModules", (string param, bool authorized) => {
                List<string> modules = new();

                foreach (KeyValuePair<string, Utilities.Pair<bool, Utilities.AssemblyLoader>> entry in _loadedAssemblies)
                {
                    if (!entry.Value.Item1 || authorized)
                        modules.Add(entry.Key);
                }

                return JsonConvert.SerializeObject(modules);
            } },
            { "runModule", (string param, bool authorized) =>
            {
                try
                {
                    JObject data = (JObject)JsonConvert.DeserializeObject(param);
                    string moduleName = data["moduleName"].Value<string>();
                    string dataToRequest = data["dataToRequest"].Value<string>();
                    string requestData = data["requestData"].Value<string>();

                    Utilities.Pair<bool, Utilities.AssemblyLoader> entry = _loadedAssemblies[moduleName];
                    if (entry.Item1 && !authorized)
                        throw new Exception();
                    
                    return entry.Item2.Run(dataToRequest, new object[] { requestData });
                }
                catch 
                {
                    return "false";
                }
            } }
        };        
    
        public ModuleAPIController(ILogger<ModuleAPIController> logger)
        {
            _logger = logger;
            if (_writer == null)
                _writer = new Utilities.AsyncO(new StreamWriter($"{Constants.DataFolder}/module.dat"));
        }

        [HttpPost]
        public async Task<Response> Post([FromBody] Request requestData)
        {
            Response returnVal = new Response();

            try
            {
                if (_writer != null)
                    await _writer.WriteLine($"Unable to parse {JsonConvert.SerializeObject(requestData)}");

                returnVal.Error = false;
                returnVal.ResponseData = response[requestData.requestID](requestData.requestData, Utilities.Authenticator.IsAuthorizedKey(requestData.authenticationID));
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