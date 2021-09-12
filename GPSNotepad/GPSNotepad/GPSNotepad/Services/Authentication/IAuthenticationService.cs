using GPSNotepad.Models;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> SignInAsync(string login, string password);
        Task<bool> SignUpAsync(UserModel user);
    }
}
