using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNotepad.Services.SettingsService
{
    public class SettingsManager : ISettingsManager
    {
        private ISettings _settings;

        public SettingsManager(ISettings settings)
        {
            _settings = settings;
        }

        #region --- Implement Interface --- 

        public void AddOrUpdateUserId(int id)
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
