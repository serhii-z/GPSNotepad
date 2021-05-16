using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace GPSNotepad.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        #region --- IPermissionService implementation ---

        [Obsolete]
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

        [Obsolete]
        private async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                Device.OpenUri(new Uri("app-settings:"));
            }

            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        #endregion
    }
}
