#nullable enable

using System;

namespace OrionServer.Data
{
    public class Location
    {
        public DateTime Date { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? Notice { get; set; }
    }
}