using GpsNotepad.Services.Authorization;
using GpsNotepad.Views;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class SignInPageViewModel : ViewModelBase
    {
        private IAuthorizationService _authorizationService;

        public SignInPageViewModel(INavigationService navigationService,
                                   IAuthorizationService authorizationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
        }

        #region --- Public Properties ---

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public ICommand SignInTapCommand => new Command(OnSignInTap);
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        #endregion


        #region --- Private Helpers ---

        private async void OnSignInTap()
        {
            var isAuthorized = await _authorizationService.SignInAsync(email, password);
            if (isAuthorized)
            {
                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapPage)}");
            }
        }

        private async void OnSignUpTap()
        {
            await _navigationService.NavigateAsync($"{nameof(SignUpPage)}");
        }

        #endregion

    }
}
