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
    class CreateAccountFirstPageViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;

        public CreateAccountFirstPageViewModel(IAuthorizationService authorizationService,
                                   ILocalizationService localizationService,
                                   INavigationService navigationService) : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
        }

        #region --- Public Properties ---

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private bool isNextButtonEnabled = false;
        public bool IsNextButtonEnabled
        {
            get => isNextButtonEnabled;
            set => SetProperty(ref isNextButtonEnabled, value);
        }

        private ICommand nextTapCommand;
        public ICommand NextTapCommand => 
            nextTapCommand ?? (nextTapCommand = new DelegateCommand(OnNextTap));

        private ICommand signInWithFacebookTapCommand;
        public ICommand SignInWithFacebookTapCommand =>
            signInWithFacebookTapCommand ?? (signInWithFacebookTapCommand = new DelegateCommand(OnSignInWithFacebookTapCommandTap));


        #endregion

        #region --- Private helpers ---

        private void ClearEntries()
        {
            Name = string.Empty;
            Email = string.Empty;
        }

        private void ShowAlert(string msg)
        {
            UserDialogs.Instance.Alert(msg, Resource["Alert"], "OK");
        }

        private bool HasValidName()
        {
            bool isNameValid = true;
            if (!Validator.HasValidName(Name))
            {
                ShowAlert(Resource["HasValidName"]);
                ClearEntries();
                isNameValid = false;
            }
            return isNameValid;
        }

        private bool HasValidEmail()
        {
            bool isEmailValid = true;
            if (!Validator.HasValidEmail(Email))
            {
                ShowAlert(Resource["HasValidEmail"]);
                ClearEntries();
                isEmailValid = false;
            }
            return isEmailValid;
        }

        private UserModel CreateUser()
        {
            UserModel userModel = new UserModel()
            {
                Name = Name,
                Email = Email
            };

            return userModel;
        }

        private async void OnNextTap()
        {
            if (HasValidName() &&
                HasValidEmail())
            {
                var userModel = CreateUser();
                if (userModel != null)
                {
                    var isBusy = await _authorizationService.HasEmail(Email);
                    if (!isBusy)
                    {
                        var parameters = new NavigationParameters();
                        parameters.Add($"{nameof(UserModel)}", userModel);
                        await NavigationService.NavigateAsync($"{nameof(CreateAccountSecondPage)}", parameters);
                    }
                    else
                    {
                        ShowAlert(Resource["HasBusyEmail"]);
                        ClearEntries();
                    }
                }
            }
        }

        private async void OnSignInWithFacebookTapCommandTap()
        {
            await _authorizationService.SignInWithFacebook();
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email))
            {
                IsNextButtonEnabled = false;
            }
            else
            {
                IsNextButtonEnabled = true;
            }
        }

        #endregion

    }
}
