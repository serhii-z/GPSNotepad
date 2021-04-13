using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Services.GeoLocations
{
    public class GeoLocationService : IGeoLocationService
    {
        public async Task<Position> GetPositionAsync()
        {
           var location = await Geolocation.GetLastKnownLocationAsync();
           var userPosition = new Position(location.Latitude, location.Longitude);

            return userPosition;
        }
    }
}
