using GPSNotepad.Models;
using GPSNotepad.Services.Repositiry;
using GPSNotepad.Services.SettingsService;
using System.Linq;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private IRepositoryService _repository;
        private ISettingsManager _settingsManager;

        public AuthorizationService(IRepositoryService repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }

        #region -- IAuthorizationService implement --

        public async Task<bool> AuthorizeUserAsync(string login)
        {
            var isAuthorized = false;
            var users = await _repository.GetAllAsync<UserModel>();
            var user = users.Where(x => x.Email == login).ToList();

            if (user.Count > 0)
            {
                _settingsManager.AddUserId(user[0].Id);
                isAuthorized = true;
            }

            return isAuthorized;
        }

        public bool IsAuthorized 
        { 
            get => _settingsManager.GetUserId() > 0;
        }

        #endregion
    }
}
