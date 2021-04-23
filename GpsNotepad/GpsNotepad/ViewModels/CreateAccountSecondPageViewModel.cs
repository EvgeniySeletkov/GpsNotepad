using Acr.UserDialogs;
using GpsNotepad.Models;
using GpsNotepad.Resources;
using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Validation;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class CreateAccountSecondPageViewModel : BaseViewModel
    {
        private UserModel _userModel;

        private IAuthorizationService _authorizationService;

        public CreateAccountSecondPageViewModel(IAuthorizationService authorizationService,
                                                ILocalizationService localizationService,
                                                INavigationService navigationService) : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
        }

        #region --- Public Properties ---

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        
        //use triggers in xaml
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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        private string confirmPasswordVisibleImage = "ic_eye_off.png";
        public string ConfirmPasswordVisibleImage
        {
            get => confirmPasswordVisibleImage;
            set => SetProperty(ref confirmPasswordVisibleImage, value);
        }

        private bool isConfirmPassword = true;
        public bool IsConfirmPassword
        {
            get => isConfirmPassword;
            set => SetProperty(ref isConfirmPassword, value);
        }

        private string confirmPasswordWrongText;
        public string ConfirmPasswordWrongText
        {
            get => confirmPasswordWrongText;
            set => SetProperty(ref confirmPasswordWrongText, value);
        }

        private bool isConfirmPasswordWrongVisible = false;
        public bool IsConfirmPasswordWrongVisible
        {
            get => isConfirmPasswordWrongVisible;
            set => SetProperty(ref isConfirmPasswordWrongVisible, value);
        }

        private bool isButtonEnable = false;
        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set => SetProperty(ref isButtonEnable, value);
        }

        private ICommand passwordVisibleTapCommand;
        public ICommand PasswordVisibleTapCommand =>
            passwordVisibleTapCommand ?? (passwordVisibleTapCommand = new DelegateCommand(OnPasswordVisibleTap));

        private ICommand confirmPasswordVisibleTapCommand;
        public ICommand ConfirmPasswordVisibleTapCommand =>
            confirmPasswordVisibleTapCommand ?? (confirmPasswordVisibleTapCommand = new DelegateCommand(OnConfirmPasswordVisibleTap));

        private ICommand createAccountTapCommand;
        public ICommand CreateAccountTapCommand => 
            createAccountTapCommand ?? (createAccountTapCommand = new DelegateCommand(OnCreateAccountTap));

        private ICommand signInWithFacebookTapCommand;
        public ICommand SignInWithFacebookTapCommand =>
            signInWithFacebookTapCommand ?? (signInWithFacebookTapCommand = new DelegateCommand(OnSignInWithFacebookTapCommandTap));

        #endregion

        #region --- Private helpers ---

        private void ClearEntries()
        {
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private void ShowAlert(string msg)
        {
            UserDialogs.Instance.Alert(msg, Resource["Alert"], "OK");
        }

        private bool HasValidPassword()
        {
            bool isPasswordValid = true;
            if (!Validator.HasValidPassword(Password))
            {
                ShowAlert(Resource["HasValidPassword"]);
                ClearEntries();
                isPasswordValid = false;
            }
            if (!Validator.HasEqualPasswords(Password, ConfirmPassword))
            {
                IsConfirmPasswordWrongVisible = true;
                ConfirmPasswordWrongText = Resource["HasMatchPasswords"];
                isPasswordValid = false;
            }
            else
            {
                IsConfirmPasswordWrongVisible = false;
            }
            return isPasswordValid;
        }

        private void CreateUser()
        {
            if (!Password.Equals(_userModel.Name))
            {
                _userModel.Password = Password;
            }
            else
            {
                ShowAlert(Resource["HasEqualNameAndPassword"]);
                ClearEntries();
            }
        }

        private void OnPasswordVisibleTap()
        {
            
            PasswordWrongText = "Wrong";
            IsPassword = !IsPassword;
            if (IsPassword)
            {
                PasswordVisibleImage = "ic_eye_off.png";
                IsPasswordWrongVisible = false;
            }
            else
            {
                PasswordVisibleImage = "ic_eye.png";
                IsPasswordWrongVisible = true;
            }
        }

        private void OnConfirmPasswordVisibleTap()
        {
            IsConfirmPasswordWrongVisible = true;
            ConfirmPasswordWrongText = "Wrong";
            IsConfirmPassword = !IsConfirmPassword;
            if (IsConfirmPassword)
            {
                ConfirmPasswordVisibleImage = "ic_eye_off.png";
            }
            else
            {
                ConfirmPasswordVisibleImage = "ic_eye.png";
            }
        }

        private async void OnCreateAccountTap()
        {
            if (HasValidPassword())
            {
                CreateUser();
                if (_userModel != null)
                {
                    var parameters = new NavigationParameters();
                    await _authorizationService.CreateAccount(_userModel);
                    parameters.Add($"{nameof(UserModel)}", _userModel);
                    await NavigationService.NavigateAsync($"{nameof(LogInPage)}", parameters);
                }
            }
        }

        //rename
        private async void OnSignInWithFacebookTapCommandTap()
        {
            await _authorizationService.SignInWithFacebook();
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //trygetvalue
            _userModel = parameters.GetValue<UserModel>($"{nameof(UserModel)}");
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
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
