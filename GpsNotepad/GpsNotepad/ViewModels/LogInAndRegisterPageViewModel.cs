using GpsNotepad.Services.Localization;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class LogInAndRegisterPageViewModel : BaseViewModel
    {
        public LogInAndRegisterPageViewModel(INavigationService navigationService,
                                             ILocalizationService localizationService) : base(navigationService, localizationService)
        {

        }

        private ICommand logInTapCommand;
        public ICommand LogInTapCommand => 
            logInTapCommand ?? (logInTapCommand = new DelegateCommand(OnLogInTap));

        private ICommand createAccountTapCommand;
        public ICommand CreateAccountTapCommand =>
            createAccountTapCommand ?? (createAccountTapCommand = new DelegateCommand(OnCreateAccountTap));

        private async void OnLogInTap()
        {
            await NavigationService.NavigateAsync($"{nameof(LogInPage)}");
        }

        private async void OnCreateAccountTap()
        {
            await NavigationService.NavigateAsync($"{nameof(CreateAccountFirstPage)}");
        }
    }
}
