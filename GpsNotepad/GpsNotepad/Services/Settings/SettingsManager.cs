using System;
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
    }
}
