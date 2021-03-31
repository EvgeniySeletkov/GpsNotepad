using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }

        #region --- Public properties ---

        public ICommand SaveSettingsTapCommand => new Command(OnSaveSettingsTap);

        #endregion

        #region --- Private helpers ---

        private async void OnSaveSettingsTap()
        {
            await _navigationService.GoBackAsync();
        }

        #endregion
    }
}
