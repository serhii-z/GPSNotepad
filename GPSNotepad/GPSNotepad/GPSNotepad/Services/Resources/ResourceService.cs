using GPSNotepad.Resources.Images;
using GPSNotepad.Resources.Localization;
using GPSNotepad.Resources.Themes;
using GPSNotepad.Services.SettingsService;
using System.IO;
using System.Reflection;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Services.Resources
{
    public class ResourceService : IResourceService
    {
        private ISettingsManager _settingsManager;

        public ResourceService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region -- IResourceService implement --

        public void ApplyTheme()
        {
            switch (_settingsManager.ThemeName)
            {
                case nameof(DarkTheme):
                    App.Current.Resources.Add(new DarkTheme());
                    ApplyDarkColor();
                    break;
                default:
                    App.Current.Resources.Add(new LightTheme());
                    UndoDarkColor();
                    break;
            }
        }

        public void ApplyCulture()
        {
            switch (_settingsManager.CultureName)
            {
                case "ru":
                    App.Current.Resources.Add(new Ru());
                    ApplyDarkColor();
                    break;
                default:
                    App.Current.Resources.Add(new En());
                    UndoDarkColor();
                    break;
            }
        }

        public MapStyle GetMapStyle()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = Stream.Null;
            var styleFile = string.Empty;

            if (_settingsManager.ThemeName == Constants.DarkTheme)
            {
                stream = assembly.GetManifestResourceStream(Constants.DarkThemePath);
            }
            else
            {
                stream = assembly.GetManifestResourceStream(Constants.LightThemePath);
            }

            using (var reader = new StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            return MapStyle.FromJson(styleFile);
        }

        public void AddIcons()
        {
            App.Current.Resources.Add(new Icons());
        }

        #endregion


        #region -- Private methods --

        private void ApplyDarkColor()
        {
            _settingsManager.ColorClock = Constants.DarkTheme;
        }

        private void UndoDarkColor()
        {
            if (_settingsManager.IsRadioButtonBlue)
            {
                _settingsManager.ColorClock = Constants.ColorClockDefault;
            }
            if (_settingsManager.IsRadioButtonRed)
            {
                _settingsManager.ColorClock = Constants.ColorClockRed;
            }
        }

        #endregion
    }
}
