using Plugin.Settings.Abstractions;

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

        public void AddUserId(int id)
        {
            _settings.AddOrUpdateValue("id", id);
        }

        public int GetUserId()
        {
            return _settings.GetValueOrDefault("id", 0);
        }

        #endregion
    }
}
