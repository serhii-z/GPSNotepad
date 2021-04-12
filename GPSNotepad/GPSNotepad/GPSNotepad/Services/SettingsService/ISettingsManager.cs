namespace GPSNotepad.Services.SettingsService
{
    public interface ISettingsManager
    {
        void AddOrUpdateUserId(int id);
        int GetUserId();
    }
}
