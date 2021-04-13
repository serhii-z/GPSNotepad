using System.Threading.Tasks;

namespace GPSNotepad.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> AuthorizeUserAsync(string login);
        bool IsAuthorized { get; }
    }
}
