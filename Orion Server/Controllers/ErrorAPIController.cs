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
    public class ErrorAPIController : ControllerBase
    {
        [HttpPost]
        public async Task<Response> Post([FromBody] Request requestData)
        {
            Response returnVal = new Response();
            try
            {
                Random random = new Random();
                await System.IO.File.WriteAllTextAsync($"{Constants.ErrorSubmitions}/{new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 27).Select(s => s[random.Next(s.Length)]).ToArray())}", requestData.RequestData);
                returnVal.Error = false;
                returnVal.ResponseData = "true";
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