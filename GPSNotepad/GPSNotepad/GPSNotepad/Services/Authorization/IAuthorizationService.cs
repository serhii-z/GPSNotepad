namespace GPSNotepad.Services.Authorization
{
    public interface IAuthorizationService
    {
        bool IsAuthorized { get; }
        void LogOut();
    }
}
