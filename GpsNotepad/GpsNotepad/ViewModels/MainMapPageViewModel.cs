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
    class MainMapPageViewModel : BaseViewModel
    {
        private ISettingsManager _settingsManager;

        public MainMapPageViewModel(INavigationService navigationService,
                                    ISettingsManager settingsManager) : base(navigationService)
        {
            _navigationService = navigationService;
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
