using Microsoft.Extensions.Logging;

namespace OrionServer.Controllers.Data
{
        public class PressureController : Generic.GenericDataController<OrionServer.Data.Pressure, PressureController>
    {
        public PressureController(ILogger<PressureController> logger) : base(logger) { }
    }
}