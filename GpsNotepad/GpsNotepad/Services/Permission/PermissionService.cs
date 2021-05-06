using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GpsNotepad.Services.Permission
{
    class PermissionService : IPermissionService
    {
        public async Task<PermissionStatus> RequestLocationPermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Denied)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }


            return status;
        }
    }
}
