using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.MapCameraPosition
{
    interface IMapCameraPositionService
    {
        MapSpan GetCameraPosition();

        void SaveCameraPosition(CameraPosition cameraPosition);

    }
}
