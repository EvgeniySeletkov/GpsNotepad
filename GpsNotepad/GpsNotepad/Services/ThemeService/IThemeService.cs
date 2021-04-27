using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GpsNotepad.Services.ThemeService
{
    interface IThemeService
    {
        void SetTheme(OSAppTheme theme);
        string GetTheme();
    }
}
