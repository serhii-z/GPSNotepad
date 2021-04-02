using Xamarin.Essentials;

namespace GPSNotepad.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        public int UserId 
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value); 
        }
    }
}
