using GPSNotepad.Services.SettingsService;

namespace GPSNotepad.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private ISettingsManager _settingsManager;

        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region -- IAuthorizationService implement --

        public bool IsAuthorized 
        { 
            get => _settingsManager.GetUserId() > 0;
        }

        public void LogOut()
        {
            _settingsManager.SaveUserId(0);
        }

        #endregion
    }
}
