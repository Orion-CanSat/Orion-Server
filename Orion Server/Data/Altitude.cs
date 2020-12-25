#nullable enable

using System;

namespace OrionServer.Data
{
    public class Altitude
    {
        public DateTime Date { get; set; }
        public double AltitudeM { get; set; }
        public double AltitudeF => AltitudeM * 3.2808;
        public string? Notice { get; set; }
    }
}