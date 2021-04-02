using GPSNotepad.Models;
using GPSNotepad.Services.Repositiry;
using System.Collections.Generic;
using System.Linq;

namespace GPSNotepad.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public int VerifyUser(string login, string password)
        {
            var user = GetAllUsers().Where(x => x.Email == login && x.Password == password).ToList();

            if (user == null)
            {
                return 0;
            }

            return user[0].Id;
        }

        public bool IsLogin(string login)
        {
            var user = GetAllUsers().Where(x => x.Email == login).ToList();

            if (user == null)
            {
                return false;
            }

            return true;
        }

        public int AddUser(UserModel user)
        {
            var result = Repository.InsertAsync(user).Result;

            return result;
        }

        private List<UserModel> GetAllUsers()
        {
            var users = Repository.GetAllAsync<UserModel>().Result;

            return users;
        }
    }
}
