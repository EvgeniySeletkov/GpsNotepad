using GpsNotepad.Resources;
using GpsNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GpsNotepad.Services.Localization
{
    class LocalizationService : ILocalizationService
    {
        private ISettingsManager _settingsManager;

        public LocalizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public void SetLocalization()
        {
            var cultureInfo = new CultureInfo(_settingsManager.Culture, false);
            Resource.Culture = cultureInfo;
        }
    }
}
