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
    [ApiController]
    [Route("api/[controller]")]
    public class AdminAPIController : ControllerBase
    {
        private readonly ILogger<AdminAPIController> _logger;
        private static Utilities.AsyncO? _writer = null;

        private Dictionary<string, Func<string, object>> response = new Dictionary<string, Func<string, object>>()
        {
            { "getAllKeys", (string param) => { return JsonConvert.SerializeObject(Utilities.Authenticator.GetAllKeys()); }},
            { "addKey", (string param) => { Utilities.Authenticator.AuthorizeKey(param); return "true"; } },
            { "removeKey", (string param) => { Utilities.Authenticator.RemoveKey(param); return "true"; } },
            { "getAllModules", (string param) => {
                string[] modulesName = Utilities.AssemblyLoader.GetAssemblies();
                List<Tuple<string, string, bool>> modulesNameWithHashAndLoaded = new();

                for (int i = 0; i < modulesName.Length; i++)
                    modulesNameWithHashAndLoaded.Add(new Tuple<string, string, bool>(Utilities.Crypto.MD5.CreateMD5(modulesName[i]).Substring(0, 10), modulesName[i], ModuleAPIController._loadedAssemblies.ContainsKey(modulesName[i])));

                return JsonConvert.SerializeObject(modulesNameWithHashAndLoaded);
            } },
            { "loadModule", (string param) => {
                if (ModuleAPIController._loadedAssemblies.ContainsKey(param))
                    return "true";
                else
                {
                    try
                    {
                        Utilities.AssemblyLoader al = new Utilities.AssemblyLoader(param);
                        ModuleAPIController._loadedAssemblies.Add(param, new Utilities.Pair<bool, Utilities.AssemblyLoader>(true, al));
                        return "true";
                    }
                    catch
                    {
                        return "false";
                    }
                }
            } },
            { "unloadModule", (string param) => {
                if (!ModuleAPIController._loadedAssemblies.ContainsKey(param))
                    return "true";
                ModuleAPIController._loadedAssemblies.Remove(param);
                return "true";
            } },
            { "deleteModule", (string param) => { return ((Utilities.AssemblyLoader.AssemblyRemove(param)) ? "true" : "false"); } }
        };

        public AdminAPIController(ILogger<AdminAPIController> logger)
        {
            _logger = logger;
            if (_writer == null)
                _writer = new Utilities.AsyncO(new StreamWriter($"{Constants.DataFolder}/admin.dat"));
        }

        [HttpPost]
        public async Task<Response> Post([FromBody] Request requestData)
        {
            Response returnVal = new Response();
            try
            {
                if (!Utilities.Authenticator.IsAuthorizedKey(requestData.authenticationID))
                {
                    returnVal.Error = true;
                    returnVal.ErrorMessage = "Unauthorized User";
                }
                else
                {
                    if (_writer != null)
                        await _writer.WriteLine($"+ {JsonConvert.SerializeObject(requestData)}");
                    returnVal.Error = false;
                    returnVal.ResponseData = response[requestData.requestID](requestData.requestData).ToString();
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