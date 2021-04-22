using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
using GpsNotepad.ViewModels.ExtendedViewModels;
using GpsNotepad.Views;
using Prism.Commands;
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

        private ICommand exitTapCommand;
        public ICommand ExitTapCommand => 
            exitTapCommand ?? (exitTapCommand = new DelegateCommand(OnExitTap));

        private ICommand settingsTapCommand;
        public ICommand SettingsTapCommand => 
            settingsTapCommand ?? (settingsTapCommand = new DelegateCommand(OnSettingsTap));

        #endregion

        #region --- Private Helpers ---

        private async void OnExitTap()
        {
            _settingsManager.UserId = 0;
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LogInAndRegisterPage)}");
        }

        private async void OnSettingsTap()
        {
            await NavigationService.NavigateAsync($"{nameof(SettingsPage)}");
        }

        #endregion

    }
}
