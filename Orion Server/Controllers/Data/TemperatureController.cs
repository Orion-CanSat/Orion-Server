using Microsoft.Extensions.Logging;
using System.IO;

namespace OrionServer.Controllers.Data
{
    public class TemperatureController : Generic.GenericDataController<OrionServer.Data.Temperature, TemperatureController>
    {
        private static readonly StreamWriter _writer = new($"{Constants.DataFolder}/temperature.dat");
        public TemperatureController(ILogger<TemperatureController> logger) : base(logger, _writer) { }
    }
}