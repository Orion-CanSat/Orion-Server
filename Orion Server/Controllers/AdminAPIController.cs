#nullable enable

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
        private static Dictionary<string, Utilities.AssemblyLoader> _loadedAssemblies = new();
        private static Utilities.AsyncO? _writer = null;

        private Dictionary<string, Func<string, string>> response = new Dictionary<string, Func<string, string>>()
        {
            { "getAllKeys", (string param) => { return JsonConvert.SerializeObject(Utilities.Authenticator.GetAllKeys()); }},
            { "addKey", (string param) => { Utilities.Authenticator.AuthorizeKey(param); return $"{{\"response\": true }}"; } },
            { "removeKey", (string param) => { Utilities.Authenticator.RemoveKey(param); return "{\"response\": true}"; } },
            { "getAllModules", (string param) => {
                string[] modulesName = Utilities.AssemblyLoader.GetAssemblies();
                List<Tuple<string, string, bool>> modulesNameWithHashAndLoaded = new();

                for (int i = 0; i < modulesName.Length; i++)
                    modulesNameWithHashAndLoaded.Add(new Tuple<string, string, bool>(Utilities.Crypto.MD5.CreateMD5(modulesName[i]).Substring(0, 10), modulesName[i], _loadedAssemblies.ContainsKey(modulesName[i])));

                return JsonConvert.SerializeObject(modulesNameWithHashAndLoaded);
            } },
            { "loadModule", (string param) => {
                if (_loadedAssemblies.ContainsKey(param))
                    return $"{{\"response\": true }}";
                else
                {
                    try
                    {
                        Utilities.AssemblyLoader al = new Utilities.AssemblyLoader(param);
                        _loadedAssemblies.Add(param, al);
                        return $"{{\"response\": true }}";
                    }
                    catch
                    {
                        return $"{{\"response\": false }}";
                    }
                }
            } },
            { "unloadModule", (string param) => {
                if (!_loadedAssemblies.ContainsKey(param))
                    return $"{{\"response\": true }}";
                _loadedAssemblies.Remove(param);
                return $"{{\"response\": true }}";
            } },
            { "deleteModule", (string param) => {return $"{{\"response\": " + ((Utilities.AssemblyLoader.AssemblyRemove(param)) ? "true" : "false") + " }"; } }
        };

        public AdminAPIController(ILogger<AdminAPIController> logger)
        {
            _logger = logger;
            if (_writer != null)
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
                    if (_writer != null)
                        await _writer.WriteLine($"+ {JsonConvert.SerializeObject(requestData)}");
                    returnVal = response[requestData.requestID](requestData.requestData);
                }
            }
            catch { }
            return returnVal;
        }
    }
}