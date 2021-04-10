#nullable enable

using System;

namespace OrionServer.Data
{
    public class Status
    {
        public DateTime Date { get; set; }
        // -1: Unknown
        //  0: Off
        //  1: On
        public int Stat { get; set; }
        public string? Notice { get; set; }
    }
}