using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
using GpsNotepad.Services.ThemeService;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class SettingsPageViewModel : BaseViewModel
    {
        private readonly IThemeService _themeService;

        public SettingsPageViewModel(INavigationService navigationService,
                                     ILocalizationService localizationService,
                                     IThemeService themeService) : base(navigationService, localizationService)
        {
            _themeService = themeService;
        }

        #region --- Public properties ---

        private bool _darkTheme;
        public bool DarkTheme
        {
            get => _darkTheme;
            set => SetProperty(ref _darkTheme, value);
        }

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

        private ICommand _openLanguageSettingsTapCommand;
        public ICommand OpenLanguageSettingsTapCommand =>
            _openLanguageSettingsTapCommand ??= new DelegateCommand(OnOpenLanguageSettingsTapAsync);

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

        private async void OnOpenLanguageSettingsTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(LanguageSettingsPage));
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
            var theme = _themeService.GetTheme();
            DarkTheme = theme == OSAppTheme.Dark.ToString();
            ActivateLanguageControl();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(DarkTheme))
            {
                var theme = DarkTheme ? OSAppTheme.Dark : OSAppTheme.Light;
                _themeService.SetTheme(theme);
            }
        }

        #endregion
    }
}
