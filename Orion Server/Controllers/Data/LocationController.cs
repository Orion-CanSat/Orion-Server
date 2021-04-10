using Microsoft.Extensions.Logging;
using System.IO;

namespace OrionServer.Controllers.Data
{
    public class LocationController : Generic.GenericDataController<OrionServer.Data.Location, LocationController>
    {
        private static readonly StreamWriter _writer = new($"{Constants.DataFolder}/location.dat");
        public LocationController(ILogger<LocationController> logger) : base(logger, _writer) { }
    }
}