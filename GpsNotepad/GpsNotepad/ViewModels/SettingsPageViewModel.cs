using GpsNotepad.Constants;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class SettingsPageViewModel : BaseViewModel
    {
        private ISettingsManager _settingsManager;
        private ILocalizationService _localizationService;

        public SettingsPageViewModel(INavigationService navigationService,
                                     ILocalizationService localizationService,
                                     ISettingsManager settingsManager) : base(navigationService, localizationService)
        {
            _settingsManager = settingsManager;
            _localizationService = localizationService;
        }

        #region --- Public properties ---

        private string selectedTheme;
        public string SelectedTheme
        {
            get => selectedTheme;
            set => SetProperty(ref selectedTheme, value);
        }

        private string selectedLang;
        public string SelectedLang
        {
            get => selectedLang;
            set => SetProperty(ref selectedLang, value);
        }

        public ICommand SaveSettingsTapCommand => new Command(OnSaveSettingsTap);

        #endregion

        #region --- Private Methods ---

        private void ActivateLanguageControl()
        {
            //SelectedLang = _localizationService.Lang;

            switch (_localizationService.Lang)
            {
                case Constant.ENGLISH_LANGUAGE:
                    SelectedLang = Constant.ENGLISH_LANGUAGE;
                    break;
                case Constant.RUSSIAN_LANGUAGE:
                    SelectedLang = Constant.RUSSIAN_LANGUAGE;
                    break;
            }

        }

        private void SaveLanguageSettings()
        {
            Resource.Lang = SelectedLang;
            Resource.SetCulture(SelectedLang);
        }

        #endregion

        #region --- Private helpers ---

        private async void OnSaveSettingsTap()
        {
            SaveLanguageSettings();
            await _navigationService.GoBackAsync();
        }

        #endregion

        #region --- Overrides ---

        public override void Initialize(INavigationParameters parameters)
        {
            ActivateLanguageControl();
        }

        #endregion
    }
}
