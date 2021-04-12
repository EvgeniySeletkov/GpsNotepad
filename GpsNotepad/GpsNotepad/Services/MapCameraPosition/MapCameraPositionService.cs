using GpsNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
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

        public void SetCameraPosition(CameraPosition cameraPosition)
        {
            _settingsManager.Latitude = cameraPosition.Target.Latitude;
            _settingsManager.Longitude = cameraPosition.Target.Longitude;
            _settingsManager.Zoom = Math.Round(cameraPosition.Zoom);
        }
    }
}
