namespace GPSNotepad.Services.SettingsService
{
    public interface ISettingsManager
    {
        void SaveUserId(int id);
        int GetUserId();
    }
}
