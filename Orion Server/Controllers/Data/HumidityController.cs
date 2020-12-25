using Microsoft.Extensions.Logging;

namespace OrionServer.Controllers.Data
{
        public class HumidityController : Generic.GenericDataController<OrionServer.Data.Humidity, HumidityController>
    {
        public HumidityController(ILogger<HumidityController> logger) : base(logger) { }
    }
}