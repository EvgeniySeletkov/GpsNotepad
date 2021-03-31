using GpsNotepad.Models;
using GpsNotepad.Services.Authorization;
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
    class SignUpPageViewModel : ViewModelBase
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
            set => SetProperty(ref name, value);
        }

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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        #endregion

        #region --- Private Methods ---

        private UserModel CreateUser()
        {
            UserModel userModel = null;

            userModel = new UserModel()
            {
                Name = Name,
                Email = Email,
                Password = Password
            };

            return userModel;
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSignUpTap(object obj)
        {
            var userModel = CreateUser();
            if (userModel != null)
            {
                await _authorizationService.SignUp(userModel);
                await _navigationService.GoBackAsync();
            }
        }

        #endregion
    }
}
