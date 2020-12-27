using Microsoft.Extensions.Logging;
using System.IO;

namespace OrionServer.Controllers.Data
{
    public class HumidityController : Generic.GenericDataController<OrionServer.Data.Humidity, HumidityController>
    {
        private static readonly StreamWriter _writer = new($"{Constants.DataFolder}/humidity.dat");
        public HumidityController(ILogger<HumidityController> logger) : base(logger, _writer) { }
    }
}