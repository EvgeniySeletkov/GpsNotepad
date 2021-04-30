using GpsNotepad.Services.Settings;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.MapCameraPosition
{
    class MapCameraPositionService : IMapCameraPositionService
    {
        private ISettingsManager _settingsManager;

        public MapCameraPositionService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public MapSpan GetCameraPosition()
        {
            var position = new Position(_settingsManager.Latitude, _settingsManager.Longitude);
            var mapSpan = new MapSpan(position, 1, 1);
            return mapSpan;
        }

        public void SaveCameraPosition(CameraPosition cameraPosition)
        {
            _settingsManager.Latitude = cameraPosition.Target.Latitude;
            _settingsManager.Longitude = cameraPosition.Target.Longitude;
        }
    }
}
