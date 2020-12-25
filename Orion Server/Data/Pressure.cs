#nullable enable

using System;

namespace OrionServer.Data
{
    public class Pressure
    {
        public DateTime Date { get; set; }
        public double PressurePa { get; set; }
        public double PressurekPa => PressurePa / 1000;
        public string? Notice { get; set; }
    }
}