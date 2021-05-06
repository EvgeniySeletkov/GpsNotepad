using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GpsNotepad.Services.Permission
{
    interface IPermissionService
    {
        Task<PermissionStatus> RequestLocationPermissionAsync();
    }
}
