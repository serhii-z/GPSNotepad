using GPSNotepad.Models;
using GPSNotepad.Services.Repositiry;
using System.Linq;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private IRepositoryService _repository;

        public AuthenticationService(IRepositoryService repository)
        {
            _repository = repository;
        }

        #region -- Interface IAuthenticationService implementation --

        public async Task<bool> SignInAsync(string login, string password)
        {
            var users = await _repository.GetAllAsync<UserModel>();
            var user = users.Where(x => x.Email == login && x.Password == password).ToList();
            var isMatch = false;

            if (user.Count > 0)
            {
                isMatch = true;
            }

            return isMatch;
        }

        public async Task<bool> SignUpAsync(UserModel newUser)
        {
            var isSignUp = false;
            var users = await _repository.GetAllAsync<UserModel>();
            var user = users.Where(x => x.Email == newUser.Email).ToList();

            if (user.Count == 0)
            {
                var result = await _repository.InsertAsync(newUser);
                isSignUp = true;
            }

            return isSignUp;
        }

        #endregion
    }
}

