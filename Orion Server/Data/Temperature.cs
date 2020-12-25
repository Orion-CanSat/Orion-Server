using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrionServer.Data
{
    public class Temperature
    {
        public DateTime Date { get; set; }
        public double TemperatureC { get; set; }
        public double TemperatureF => (TemperatureC / 0.5556) + 32;
        public string Notice { get; set; }
    }
}
