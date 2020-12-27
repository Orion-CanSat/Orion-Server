using Microsoft.Extensions.Logging;
using System.IO;

namespace OrionServer.Controllers.Data
{
    public class PressureController : Generic.GenericDataController<OrionServer.Data.Pressure, PressureController>
    {
        private static readonly StreamWriter _writer = new($"{Constants.DataFolder}/pressure.dat");
        public PressureController(ILogger<PressureController> logger) : base(logger, _writer) { }
    }
}