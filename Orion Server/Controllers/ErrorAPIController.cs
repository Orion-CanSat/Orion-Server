#nullable enable

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OrionServer.Utilities;
using System.Net;

namespace OrionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorAPIController : ControllerBase
    {
        internal static OrionServer.Data.ErrorMail? errorMail = null;
        internal static bool loadedEmail = false;

        public static async Task<bool> LoadMail()
        {
            loadedEmail = true;
            try
            {
                if (!System.IO.File.Exists(Constants.ErrorEmail))
                    return false;
                string fileContent = await System.IO.File.ReadAllTextAsync(Constants.ErrorEmail);
                ErrorAPIController.errorMail = JsonConvert.DeserializeObject<OrionServer.Data.ErrorMail>(fileContent);
                if (ErrorAPIController.errorMail.Server == "" || ErrorAPIController.errorMail.Sender == "" || ErrorAPIController.errorMail.Password == "" || ErrorAPIController.errorMail.Recipient == "")
                {
                    ErrorAPIController.errorMail = null;
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> SendEmail(string data)
        {
            try
            {
                if (ErrorAPIController.errorMail == null)
                {
                    await LoadMail();
                }

                SmtpClient client = new SmtpClient(ErrorAPIController.errorMail.Server)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(ErrorAPIController.errorMail.Sender, ErrorAPIController.errorMail.Password),
                    EnableSsl = true
                };

                client.Send(ErrorAPIController.errorMail.Sender, ErrorAPIController.errorMail.Recipient, "Orion Server error", data);

                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<Response> Post([FromBody] Request requestData)
        {
            Response returnVal = new Response();
            try
            {
                Random random = new Random();
                await System.IO.File.WriteAllTextAsync($"{Constants.ErrorSubmitions}/{new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 27).Select(s => s[random.Next(s.Length)]).ToArray())}", requestData.RequestData);
                await ErrorAPIController.SendEmail(requestData.RequestData);
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