using Microsoft.Extensions.Logging;

namespace OrionServer.Controllers.Data
{
        public class AltitudeController : Generic.GenericDataController<OrionServer.Data.Altitude, AltitudeController>
    {
        public AltitudeController(ILogger<AltitudeController> logger) : base(logger) { }
    }
}