namespace GPSNotepad.Services.SettingsService
{
    public interface ISettingsManager
    {
        void AddUserId(int id);
        int GetUserId();
    }
}
