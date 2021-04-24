﻿using Xamarin.Essentials;

namespace GpsNotepad.Services.Settings
{
    class SettingsManager : ISettingsManager
    {
        public int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }
        public string Culture
        {
            get => Preferences.Get(nameof(Culture), Constants.ENGLISH_LANGUAGE);
            set => Preferences.Set(nameof(Culture), value);
        }

        public double Latitude
        {
            get => Preferences.Get(nameof(Latitude), Constants.DEFAULT_LATITUDE);
            set => Preferences.Set(nameof(Latitude), value);
        }

        public double Longitude
        {
            get => Preferences.Get(nameof(Longitude), Constants.DEFAULT_LONGITUDE);
            set => Preferences.Set(nameof(Longitude), value);
        }

        public double Zoom
        {
            get => Preferences.Get(nameof(Zoom), 10);
            set => Preferences.Set(nameof(Zoom), value);
        }

        public void ClearSettings()
        {
            UserId = 0;
            Culture = Constants.ENGLISH_LANGUAGE;
            Latitude = Constants.DEFAULT_LATITUDE;
            Longitude = Constants.DEFAULT_LONGITUDE;
        }
    }
}
