using Xamarin.Essentials;
using Xamarin.Forms;

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

        public string Theme
        {
            get => Preferences.Get(nameof(Theme), OSAppTheme.Light.ToString());
            set => Preferences.Set(nameof(Theme), value);
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

        public void ClearSettings()
        {
            UserId = 0;
            Latitude = Constants.DEFAULT_LATITUDE;
            Longitude = Constants.DEFAULT_LONGITUDE;
        }
    }
}
