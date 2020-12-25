#nullable enable

using System;

namespace OrionServer.Data
{
    public class Humidity
    {
        public DateTime Date { get; set; }
        public double RelativeHumidity { get; set; }
        public string? Notice { get; set; }
    }
}