namespace GPSNotepad.Services.SettingsService
{
    public interface ISettingsManager
    {
        bool IsRadioButtonBlue { get; set; }
        bool IsRadioButtonRed { get; set; }
        string ColorClock { get; set; }
        bool IsDarkTheme { get; set; }
        bool IsRussianCulture { get; set; }
        string ThemeName { get; set; }
        string CultureName { get; set; }
        void SaveUserId(int id);
        int GetUserId();
    }
}
