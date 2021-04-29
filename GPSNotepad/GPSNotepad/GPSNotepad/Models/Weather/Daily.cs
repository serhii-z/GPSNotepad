using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNotepad.Models.Weather
{
    public class Daily
    {
        public Temp temp { get; set; }
        public List<Weather> weather { get; set; }
    }
}
