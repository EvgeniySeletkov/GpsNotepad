using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.MapCameraPosition
{
    interface IMapCameraPositionService
    {
        MapSpan GetCameraPosition();
        void SetCameraPosition(CameraPosition cameraPosition);

    }
}
