namespace GPSNotepad.Services.SettingsService
{
    public interface ISettingsManager
    {
        bool IsRadioButtonBlue { get; set; }
        bool IsRadioButtonRed { get; set; }
        string ColorClock { get; set; }
        bool IsDarkTheme { get; set; }
        string ThemeName { get; set; }
        void SaveUserId(int id);
        int GetUserId();
    }
}
