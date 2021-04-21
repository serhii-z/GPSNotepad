using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace GPSNotepad.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        #region --- IPermissionService implement ---

        public async Task<bool> CheckStatusAsync()
        {
            var isStatus = false;
            var status = await CheckAndRequestPermissionAsync(new LocationWhenInUse());

            if (status == PermissionStatus.Granted)
            {
                isStatus = true;
            }

            return isStatus;
        }

        #endregion

        #region --- Private Methods ---

        private async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();

            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        #endregion
    }
}
