using GpsNotepad.Models;
using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
using GpsNotepad.Views;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class LogInPageViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;

        public LogInPageViewModel(INavigationService navigationService,
                                  ILocalizationService localizationService,
                                  IAuthorizationService authorizationService) : base(navigationService, localizationService)
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

        private bool isButtonEnable = false;
        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set => SetProperty(ref isButtonEnable, value);
        }

        private ICommand signInTapCommand;
        public ICommand SignInTapCommand =>
            signInTapCommand ?? (signInTapCommand = new DelegateCommand(OnSignInTap));

        private ICommand signInWithFacebookTapCommand;
        public ICommand SignInWithFacebookTapCommand =>
            signInWithFacebookTapCommand ?? (signInWithFacebookTapCommand = new DelegateCommand(OnSignInWithFacebookTap));

        private ICommand signUpTapCommand;
        public ICommand SignUpTapCommand => 
            signUpTapCommand ?? (signUpTapCommand = new DelegateCommand(OnSignUpTap));

        #endregion

        #region --- Private helpers ---

        private async void OnSignInTap()
        {
            var isAuthorized = await _authorizationService.SignInAsync(email, password);
            if (isAuthorized)
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
            }
        }

        private async void OnSignInWithFacebookTap()
        {
            await _authorizationService.SignInWithFacebook();
        }

        private async void OnSignUpTap()
        {
            await NavigationService.NavigateAsync($"{nameof(CreateAccountFirstPage)}");
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var userModel = parameters.GetValue<UserModel>($"{nameof(UserModel)}");
            Email = userModel.Email;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                IsButtonEnable = false;
            }
            else
            {
                IsButtonEnable = true;
            }
        }

        #endregion

    }
}
