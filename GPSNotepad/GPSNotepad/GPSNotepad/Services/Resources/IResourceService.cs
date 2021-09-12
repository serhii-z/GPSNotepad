using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Services.Resources
{
    public interface IResourceService
    {
        void ApplyTheme();
        void ApplyCulture();
        MapStyle GetMapStyle();
        void AddIcons();
    }
}
