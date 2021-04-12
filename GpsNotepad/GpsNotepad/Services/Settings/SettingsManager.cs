﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

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
            get => Preferences.Get(nameof(Culture), "en");
            set => Preferences.Set(nameof(Culture), value);
        }

        public double Latitude
        {
            get => Preferences.Get(nameof(Latitude), 41.89);
            set => Preferences.Set(nameof(Latitude), value);
        }

        public double Longitude
        {
            get => Preferences.Get(nameof(Longitude), 12.49);
            set => Preferences.Set(nameof(Longitude), value);
        }

        public double Zoom
        {
            get => Preferences.Get(nameof(Zoom), 10);
            set => Preferences.Set(nameof(Zoom), value);
        }
    }
}
