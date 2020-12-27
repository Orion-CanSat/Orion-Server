using Microsoft.Extensions.Logging;
using System.IO;

namespace OrionServer.Controllers.Data
{
    public class AltitudeController : Generic.GenericDataController<OrionServer.Data.Altitude, AltitudeController>
    {
        private static readonly StreamWriter _writer = new($"{Constants.DataFolder}/altitude.dat");
        public AltitudeController(ILogger<AltitudeController> logger) : base(logger, _writer) { }
    }
}