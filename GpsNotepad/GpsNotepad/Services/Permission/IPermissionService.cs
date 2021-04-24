using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GpsNotepad.Services.Permission
{
    interface IPermissionService
    {
        //rename
        Task<PermissionStatus> RequestLocationPermissionAsync();
    }
}
