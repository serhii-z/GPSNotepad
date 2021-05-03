using System.Collections.Generic;

namespace GPSNotepad.Models.Weather
{
    public class Daily
    {
        public Temp temp { get; set; }
        public List<Weather> weather { get; set; }
    }
}
