using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNotepad.Models.Weather
{
    public class WeatherResponse
    {
        public List<Weather> Weather { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
    }
}
