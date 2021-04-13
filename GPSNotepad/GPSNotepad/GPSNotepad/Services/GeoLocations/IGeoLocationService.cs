using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Services.GeoLocations
{
    public interface IGeoLocationService
    {
        Task<Position> GetPositionAsync();
    }
}
