using Acr.UserDialogs;
using GpsNotepad.Models;
using GpsNotepad.Resources;
using GpsNotepad.Services.Authorization;
using GpsNotepad.Validation;
using GpsNotepad.Views;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class SignUpPageViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;

        public SignUpPageViewModel(IAuthorizationService authorizationService,
                                   INavigationService navigationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
        }

        #region --- Public Properties ---

        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                CheckEntries();
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);
                CheckEntries();
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                CheckEntries();
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
                CheckEntries();
            }
        }

        private bool isButtonEnable = false;
        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set => SetProperty(ref isButtonEnable, value);
        }

        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        #endregion

        #region --- Private Methods ---

        private void CheckEntries()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                IsButtonEnable = false;
            }
            else
            {
                IsButtonEnable = true;
            }
        }

        private void ClearEntries()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private void ShowAlert(string msg)
        {
            UserDialogs.Instance.Alert(msg, Resource.Alert, "OK");
        }

        private bool HasValidName()
        {
            if (Validator.HasFirstDigitalSymbol(Name))
            {
                ShowAlert(Resource.HasFirstDigitalSymbol);
                ClearEntries();
                return false;
            }
            if (!Validator.HasValidLength(Name, 4))
            {
                ShowAlert(Resource.HasNameValidLength);
                ClearEntries();
                return false;
            }
            return true;
        }

        private bool HasValidEmail()
        {
            if (!Validator.HasValidEmail(Email))
            {
                ShowAlert(Resource.HasValidEmail);
                ClearEntries();
                return false;
            }
            return true;
        }

        private bool HasValidPassword()
        {
            if (!Validator.HasValidPassword(Password))
            {
                ShowAlert(Resource.HasValidPassword);
                ClearEntries();
                return false;
            }
            if (!Validator.HasValidLength(Password, 6))
            {
                ShowAlert(Resource.HasPasswordValidLength);
                ClearEntries();
                return false;
            }
            if (!Validator.HasEqualPasswords(Password, ConfirmPassword))
            {
                ShowAlert(Resource.HasEqualPasswords);
                ClearEntries();
                return false;
            }
            return true;
        }

        private UserModel CreateUser()
        {
            UserModel userModel = null;

            if (!Password.Equals(Name))
            {
                userModel = new UserModel()
                {
                    Name = Name,
                    Email = Email,
                    Password = Password
                };
            }
            else
            {
                ShowAlert(Resource.HasEqualNameAndPassword);
                ClearEntries();
            }

            return userModel;
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSignUpTap(object obj)
        {
            if (HasValidName() &&
                HasValidEmail() &&
                HasValidPassword())
            {
                var userModel = CreateUser();
                if (userModel != null)
                {
                    var isAutorized = await _authorizationService.SignUp(userModel);
                    if (isAutorized)
                    {
                        await _navigationService.GoBackAsync();
                    }
                    else
                    {
                        ShowAlert(Resource.HasBusyEmail);
                        ClearEntries();
                    }
                }
            }
        }

        #endregion
    }
}
