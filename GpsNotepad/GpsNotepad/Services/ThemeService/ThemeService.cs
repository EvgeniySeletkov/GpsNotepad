using GpsNotepad.Services.Settings;
using Xamarin.Forms;

namespace GpsNotepad.Services.ThemeService
{
    class ThemeService : IThemeService
    {
        private readonly ISettingsManager _settingsManager;

        public ThemeService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public string GetTheme()
        {
            string theme = _settingsManager.Theme;
            return theme;
        }

        public void SetTheme(OSAppTheme theme)
        {
            Application.Current.UserAppTheme = theme;
            _settingsManager.Theme = theme.ToString();
        }
    }
}
