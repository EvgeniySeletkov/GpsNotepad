using GpsNotepad.Models;
using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Validation;
using GpsNotepad.Views;
using Plugin.FacebookClient;
using Prism.Commands;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class LogInPageViewModel : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public LogInPageViewModel(INavigationService navigationService,
                                  ILocalizationService localizationService,
                                  IAuthorizationService authorizationService) : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
            _isPassword = true;
        }

        #region --- Public Properties ---

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _emailWrongText;
        public string EmailWrongText
        {
            get => _emailWrongText;
            set => SetProperty(ref _emailWrongText, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _passwordVisibleImage;
        public string PasswordVisibleImage
        {
            get => _passwordVisibleImage;
            set => SetProperty(ref _passwordVisibleImage, value);
        }

        private bool _isPassword;
        public bool IsPassword
        {
            get => _isPassword;
            set => SetProperty(ref _isPassword, value);
        }

        private string _passwordWrongText;
        public string PasswordWrongText
        {
            get => _passwordWrongText;
            set => SetProperty(ref _passwordWrongText, value);
        }

        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get => _isButtonEnable;
            set => SetProperty(ref _isButtonEnable, value);
        }

        private ICommand _goBackTapCommand;
        public ICommand GoBackTapCommand =>
            _goBackTapCommand ??= new DelegateCommand(OnGoBackTapAsync);

        private ICommand _emailClearTapCommand;
        public ICommand EmailClearTapCommand =>
            _emailClearTapCommand ??= new DelegateCommand(OnEmailClearTap);

        private ICommand _passwordVisibleTapCommand;
        public ICommand PasswordVisibleTapCommand =>
            _passwordVisibleTapCommand ??= new DelegateCommand(OnPasswordVisibleTap);

        private ICommand _logInTapCommand;
        public ICommand LogInTapCommand =>
            _logInTapCommand ??= new DelegateCommand(OnLogInTapAsync);

        private ICommand _logInWithFacebookTapCommand;
        public ICommand LogInWithFacebookTapCommand =>
            _logInWithFacebookTapCommand ??= new DelegateCommand(OnLogInWithFacebookTapAsync);

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<UserModel>(nameof(UserModel), out var userModel))
            {
                Email = userModel.Email;
            }
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

        #region --- Private helpers ---

        private bool HasValidEmail()
        {
            bool isEmailValid;
            if (!Validator.HasValidEmail(Email))
            {
                EmailWrongText = Resource["HasValidEmail"];
                isEmailValid = false;
            }
            else
            {
                EmailWrongText = string.Empty;
                isEmailValid = true;
            }
            return isEmailValid;
        }

        private bool HasValidPassword()
        {
            bool isPasswordValid;
            if (!Validator.HasValidPassword(Password))
            {
                PasswordWrongText = Resource["HasValidPassword"];
                isPasswordValid = false;
            }
            else
            {
                PasswordWrongText = string.Empty;
                isPasswordValid = true;
            }
            return isPasswordValid;
        }

        private async void OnGoBackTapAsync()
        {
            await NavigationService.GoBackAsync();
        }

        private void OnEmailClearTap()
        {
            Email = string.Empty;
        }

        private void OnPasswordVisibleTap()
        {
            IsPassword = !IsPassword;
        }

        private async void OnLogInTapAsync()
        {
            if (HasValidEmail() && HasValidPassword())
            {
                var isAuthorized = await _authorizationService.LogInAsync(Email, Password);
                if (isAuthorized)
                {
                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                }
            }
            
        }

        private async void OnLogInWithFacebookTapAsync()
        {
            var response = await _authorizationService.LogInWithFacebookAsync();
            if (response.Status == FacebookActionStatus.Completed)
            {
                bool isAutorized = await _authorizationService.AuthorizeWithFacebookAsync();
                if (isAutorized)
                {
                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                }
            }
        }

        #endregion

    }
}
