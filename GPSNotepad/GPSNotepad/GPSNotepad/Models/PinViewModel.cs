using Xamarin.Forms.Maps;

namespace GPSNotepad.Models
{
    public class PinViewModel : Pin
    {
        public int PinId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
