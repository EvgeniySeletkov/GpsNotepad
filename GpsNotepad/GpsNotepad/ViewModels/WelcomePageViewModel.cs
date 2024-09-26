using GpsNotepad.Services.Localization;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace GpsNotepad.ViewModels
{
    class WelcomePageViewModel : BaseViewModel
    {
        public WelcomePageViewModel(INavigationService navigationService,
                                    ILocalizationService localizationService) : base(navigationService, localizationService)
        {

        }

        #region --- Public properties ---

        private ICommand logInTapCommand;
        public ICommand LogInTapCommand => 
            logInTapCommand ??= new DelegateCommand(OnLogInTapAsync);

        private ICommand createAccountTapCommand;
        public ICommand CreateAccountTapCommand =>
            createAccountTapCommand ??= new DelegateCommand(OnCreateAccountTapAsync);

        #endregion

        #region --- Private helpers ---

        private async void OnLogInTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(LogInPage));
        }

        private async void OnCreateAccountTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(CreateAccountFirstPage));
        }

        #endregion

    }
}
