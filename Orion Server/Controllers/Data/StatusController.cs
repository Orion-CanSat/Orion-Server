using Microsoft.Extensions.Logging;
using System.IO;

namespace OrionServer.Controllers.Data
{
    public class StatusController : Generic.GenericDataController<OrionServer.Data.Status, StatusController>
    {
        private static readonly StreamWriter _writer = new($"{Constants.DataFolder}/status.dat");
        public StatusController(ILogger<StatusController> logger) : base(logger, _writer) { }
    }
}