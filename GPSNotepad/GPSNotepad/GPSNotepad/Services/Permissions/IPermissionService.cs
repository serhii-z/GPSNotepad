using System.Threading.Tasks;

namespace GPSNotepad.Services.Permissions
{
    public interface IPermissionService
    {
        Task<bool> CheckStatusAsync();
    }
}
