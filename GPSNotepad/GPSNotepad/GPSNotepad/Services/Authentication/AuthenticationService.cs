using GPSNotepad.Models;
using GPSNotepad.Services.Repository;
using GPSNotepad.Services.SettingsService;
using System.Linq;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private IRepositoryService _repository;
        private ISettingsManager _settingsManager;

        public AuthenticationService(IRepositoryService repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }

        #region -- IAuthenticationService implementation --

        public async Task<bool> SignInAsync(string login, string password)
        {
            var users = await _repository.GetAllAsync<UserModel>();
            users = users.Where(x => x.Email == login && x.Password == password).ToList();

            if (users.Count > 0)
            {
                _settingsManager.SaveUserId(users[0].Id);
            }

            return users.Count > 0;
        }

        public async Task<bool> SignUpAsync(string name, string email, string password)
        {
            var users = await _repository.GetAllAsync<UserModel>();
            users = users.Where(x => x.Email == email).ToList();

            if (users.Count == 0)
            {
                var user = CreateUser(name, email, password);
                var result = await _repository.InsertAsync(user);
            }

            return users.Count == 0;
        }

        #endregion

        #region -- Private methods --

        private UserModel CreateUser(string name, string email, string password)
        {
            var user = new UserModel()
            {
                Name = name,
                Email = email,
                Password = password
            };

            return user;
        }

        #endregion
    }
}

