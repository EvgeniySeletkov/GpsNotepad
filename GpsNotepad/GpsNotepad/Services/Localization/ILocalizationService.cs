using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNotepad.Services.Localization
{
    interface ILocalizationService
    {
        public string this[string key] { get; }
        void SetCulture(string lang);
        string Lang { get; set; }
        void SetLocalization();
    }
}
