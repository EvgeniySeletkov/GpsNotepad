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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        private bool isButtonEnable = false;
        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set => SetProperty(ref isButtonEnable, value);
        }

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
                ShowAlert(Resource["HasEqualPasswords"]);
                ClearEntries();
                isPasswordValid = false;
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

        private async void OnSignInWithFacebookTapCommandTap()
        {
            await _authorizationService.SignInWithFacebook();
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
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
