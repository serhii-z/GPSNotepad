namespace GPSNotepad.Models.Weather
{
    public class DataWeather
    {
        public string NameDay { get; set; }
        public string Icon { get; set; }
        public string Temp { get; set; }

        public DataWeather(string nameDay, string icon, string temp)
        {
            NameDay = nameDay;
            Icon = icon;
            Temp = temp;
        }
    }
}
