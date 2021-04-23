using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
using Prism.Commands;
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
        public SettingsPageViewModel(INavigationService navigationService,
                                     ILocalizationService localizationService) : base(navigationService, localizationService)
        {
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

        private ICommand saveSettingsTapCommand;
        public ICommand SaveSettingsTapCommand => 
            saveSettingsTapCommand ??= new DelegateCommand(OnSaveSettingsTap);

        #endregion

        #region --- Private helpers ---

        private void ActivateLanguageControl()
        {
            //try to use it
            //SelectedLang = _localizationService.Lang;

            //rename (Language)
            switch (Resource.Lang)
            {
                case Constants.ENGLISH_LANGUAGE:
                    SelectedLang = Constants.ENGLISH_LANGUAGE;
                    break;
                case Constants.RUSSIAN_LANGUAGE:
                    SelectedLang = Constants.RUSSIAN_LANGUAGE;
                    break;
            }

        }

        private void SaveLanguageSettings()
        {
            Resource.Lang = SelectedLang;
            Resource.SetCulture(SelectedLang);
        }

        private async void OnSaveSettingsTap()
        {
            SaveLanguageSettings();
            await NavigationService.GoBackAsync();
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
