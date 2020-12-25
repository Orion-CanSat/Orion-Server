using Microsoft.Extensions.Logging;

namespace OrionServer.Controllers.Data
{
    public class TemperatureController : Generic.GenericDataController<OrionServer.Data.Temperature, TemperatureController>
    {
        public TemperatureController(ILogger<TemperatureController> logger) : base(logger) { }
    }
}