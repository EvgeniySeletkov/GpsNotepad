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

        private string emailWrongText;
        public string EmailWrongText
        {
            get => emailWrongText;
            set => SetProperty(ref emailWrongText, value);
        }

        private bool isEmailWrongVisible = false;
        public bool IsEmailWrongVisible
        {
            get => isEmailWrongVisible;
            set => SetProperty(ref isEmailWrongVisible, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private string passwordVisibleImage = "ic_eye_off.png";
        public string PasswordVisibleImage
        {
            get => passwordVisibleImage;
            set => SetProperty(ref passwordVisibleImage, value);
        }

        private bool isPassword = true;
        public bool IsPassword
        {
            get => isPassword;
            set => SetProperty(ref isPassword, value);
        }

        private string passwordWrongText;
        public string PasswordWrongText
        {
            get => passwordWrongText;
            set => SetProperty(ref passwordWrongText, value);
        }

        private bool isPasswordWrongVisible = false;
        public bool IsPasswordWrongVisible
        {
            get => isPasswordWrongVisible;
            set => SetProperty(ref isPasswordWrongVisible, value);
        }

        private bool isButtonEnable = false;
        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set => SetProperty(ref isButtonEnable, value);
        }

        private ICommand emailClearTapCommand;
        public ICommand EmailClearTapCommand =>
            emailClearTapCommand ?? (emailClearTapCommand = new DelegateCommand(OnEmailClearTap));

        private ICommand passwordVisibleTapCommand;
        public ICommand PasswordVisibleTapCommand =>
            passwordVisibleTapCommand ?? (passwordVisibleTapCommand = new DelegateCommand(OnPasswordVisibleTap));

        private ICommand logInTapCommand;
        public ICommand LogInTapCommand =>
            logInTapCommand ?? (logInTapCommand = new DelegateCommand(OnLogInTap));

        private ICommand logInWithFacebookTapCommand;
        public ICommand LogInWithFacebookTapCommand =>
            logInWithFacebookTapCommand ?? (logInWithFacebookTapCommand = new DelegateCommand(OnLogInWithFacebookTap));

        private ICommand signUpTapCommand;
        public ICommand SignUpTapCommand => 
            signUpTapCommand ?? (signUpTapCommand = new DelegateCommand(OnSignUpTap));

        #endregion

        #region --- Private helpers ---

        private void OnEmailClearTap()
        {
            Email = string.Empty;
        }

        private void OnPasswordVisibleTap()
        {
            IsPassword = !IsPassword;

            passwordVisibleImage = isPassword ? "ic_eye_off.png" : "ic_eye.png";
        }

        private async void OnLogInTap()
        {
            var isAuthorized = await _authorizationService.SignInAsync(email, password);
            if (isAuthorized)
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
            }
        }

        private async void OnLogInWithFacebookTap()
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
