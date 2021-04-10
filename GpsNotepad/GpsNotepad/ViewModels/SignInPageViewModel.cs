﻿using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
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
    class SignInPageViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;

        public SignInPageViewModel(INavigationService navigationService,
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

        private bool isButtonEnable = false;
        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set => SetProperty(ref isButtonEnable, value);
        }

        public ICommand SignInTapCommand => new Command(OnSignInTap);
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        #endregion

        #region --- Private Methods ---

        private void CheckEntries()
        {
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

        #region --- Private Helpers ---

        private async void OnSignInTap()
        {
            var isAuthorized = await _authorizationService.SignInAsync(email, password);
            if (isAuthorized)
            {
                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
            }
        }

        private async void OnSignUpTap()
        {
            await _navigationService.NavigateAsync($"{nameof(SignUpPage)}");
        }

        #endregion

    }
}
