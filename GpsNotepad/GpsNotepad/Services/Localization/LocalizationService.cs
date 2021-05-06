using GpsNotepad.Resources.Strings;
using GpsNotepad.Services.Settings;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using Xamarin.Forms;

namespace GpsNotepad.Services.Localization
{
    class LocalizationService : ILocalizationService, INotifyPropertyChanged
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCultureInfo;

        public LocalizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _currentCultureInfo = new CultureInfo(_settingsManager.Culture);
            _resourceManager = new ResourceManager(typeof(Resource));

            MessagingCenter.Subscribe<object, CultureInfo>(this, string.Empty, OnCultureChanged);
        }

        public string this[string key]
        {
            get
            {
                return _resourceManager.GetString(key, _currentCultureInfo);
            }
        }

        private void OnCultureChanged(object s, CultureInfo cultureInfo)
        {
            _currentCultureInfo = cultureInfo;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetCulture(string lang)
        {
            MessagingCenter.Send<object, CultureInfo>(this, string.Empty, new CultureInfo(lang));
        }

        public string Lang
        {
            get => _settingsManager.Culture;
            set => _settingsManager.Culture = value;
        }

    }
}
