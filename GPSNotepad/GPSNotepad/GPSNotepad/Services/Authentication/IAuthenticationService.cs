using GPSNotepad.Models;

namespace GPSNotepad.Services.Authentication
{
    public interface IAuthenticationService
    {
        int VerifyUser(string login, string password);
        bool IsLogin(string login);
        int AddUser(UserModel user);
    }
}
