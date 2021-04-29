using GPSNotepad.Resources.Themes;
using Plugin.Settings.Abstractions;
using Xamarin.Essentials;

namespace GPSNotepad.Services.SettingsService
{
    public class SettingsManager : ISettingsManager
    {
        private ISettings _settings;

        public SettingsManager(ISettings settings)
        {
            _settings = settings;
        }

        #region -- ISettingsManager implementation --

        public bool IsRadioButtonBlue
        {
            get => Preferences.Get(nameof(IsRadioButtonBlue), true);
            set => Preferences.Set(nameof(IsRadioButtonBlue), value);
        }

        public bool IsRadioButtonRed
        {
            get => Preferences.Get(nameof(IsRadioButtonRed), false);
            set => Preferences.Set(nameof(IsRadioButtonRed), value);
        }

        public string ColorClock
        {
            get => Preferences.Get(nameof(ColorClock), Constants.ColorClockDefault);
            set => Preferences.Set(nameof(ColorClock), value);
        }

        public bool IsDarkTheme
        {
            get => Preferences.Get(nameof(IsDarkTheme), false);
            set => Preferences.Set(nameof(IsDarkTheme), value);
        }

        public string ThemeName
        {
            get => Preferences.Get(nameof(ThemeName), string.Empty);
            set => Preferences.Set(nameof(ThemeName), value);
        }

        public void SaveUserId(int id)
        {
            _settings.AddOrUpdateValue(Constants.UserIdKey, id);
        }

        public int GetUserId()
        {
            return _settings.GetValueOrDefault(Constants.UserIdKey, 0);
        }

        #endregion
    }
}
