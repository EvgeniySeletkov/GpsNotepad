﻿using GpsNotepad.Services.Localization;
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
    class SettingsPageViewModel : ViewModelBase
    {
        private ISettingsManager _settingsManager;
        private ILocalizationService _localizationService;

        public SettingsPageViewModel(INavigationService navigationService,
                                     ISettingsManager settingsManager,
                                     ILocalizationService localizationService) : base(navigationService)
        {
            _navigationService = navigationService;
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
            //SelectedLang = _settingsManager.Culture;

            switch (_settingsManager.Culture)
            {
                case "en":
                    SelectedLang = "en";
                    break;
                case "ru":
                    SelectedLang = "ru";
                    break;
            }

        }

        private void SaveLanguageSettings()
        {
            _settingsManager.Culture = SelectedLang;
            _localizationService.SetLocalization();
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
