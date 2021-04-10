using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
using GpsNotepad.Views;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class MainMapTabbedPageViewModel : BaseViewModel
    {
        private ISettingsManager _settingsManager;

        public MainMapTabbedPageViewModel(INavigationService navigationService,
                                          ILocalizationService localizationService,       
                                          ISettingsManager settingsManager) : base(navigationService, localizationService)
        {
            _settingsManager = settingsManager;
        }

        #region --- Public Properties ---

        public ICommand ExitTapCommand => new Command(OnExitTap);
        public ICommand SettingsTapCommand => new Command(OnSettingsTap);

        #endregion

        #region --- Private Helpers ---

        private async void OnExitTap()
        {
            _settingsManager.UserId = 0;
            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
        }

        private async void OnSettingsTap()
        {
            await _navigationService.NavigateAsync($"{nameof(SettingsPage)}");
        }

        #endregion

    }
}
