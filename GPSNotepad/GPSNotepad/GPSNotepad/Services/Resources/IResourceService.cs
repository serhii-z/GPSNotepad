using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Services.Resources
{
    public interface IResourceService
    {
        void ApplyTheme();
        MapStyle GetMapStyle();
    }
}
