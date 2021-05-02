using Xamarin.Forms;

namespace GpsNotepad.Services.ThemeService
{
    interface IThemeService
    {
        void SetTheme(OSAppTheme theme);

        string GetTheme();

    }
}
