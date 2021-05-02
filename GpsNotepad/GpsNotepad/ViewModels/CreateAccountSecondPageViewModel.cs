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
    class CreateAccountSecondPageViewModel : BaseViewModel
    {
        private UserModel _userModel;
        private readonly IAuthorizationService _authorizationService;

        public CreateAccountSecondPageViewModel(IAuthorizationService authorizationService,
                                                ILocalizationService localizationService,
                                                INavigationService navigationService) : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
            IsPassword = true;
            IsConfirmPassword = true;
        }

        #region --- Public Properties ---

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private string _confirmPasswordVisibleImage;
        public string ConfirmPasswordVisibleImage
        {
            get => _confirmPasswordVisibleImage;
            set => SetProperty(ref _confirmPasswordVisibleImage, value);
        }

        private bool _isConfirmPassword;
        public bool IsConfirmPassword
        {
            get => _isConfirmPassword;
            set => SetProperty(ref _isConfirmPassword, value);
        }

        private string _confirmPasswordWrongText;
        public string ConfirmPasswordWrongText
        {
            get => _confirmPasswordWrongText;
            set => SetProperty(ref _confirmPasswordWrongText, value);
        }

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => SetProperty(ref _isButtonEnabled, value);
        }

        private ICommand _goBackTapCommand;
        public ICommand GoBackTapCommand =>
            _goBackTapCommand ??= new DelegateCommand(OnGoBackTapAsync);

        private ICommand _passwordVisibleTapCommand;
        public ICommand PasswordVisibleTapCommand =>
            _passwordVisibleTapCommand ??= new DelegateCommand(OnPasswordVisibleTap);

        private ICommand _confirmPasswordVisibleTapCommand;
        public ICommand ConfirmPasswordVisibleTapCommand =>
            _confirmPasswordVisibleTapCommand ??= new DelegateCommand(OnConfirmPasswordVisibleTap);

        private ICommand _createAccountTapCommand;
        public ICommand CreateAccountTapCommand => 
            _createAccountTapCommand ??= new DelegateCommand(OnCreateAccountTapAsync);

        private ICommand _logInWithFacebookTapCommand;
        public ICommand LogInWithFacebookTapCommand =>
            _logInWithFacebookTapCommand ??= new DelegateCommand(OnLogInWithFacebookTapAsync);

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            parameters.TryGetValue(nameof(UserModel), out _userModel);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                IsButtonEnabled = false;
            }
            else
            {
                IsButtonEnabled = true;
            }
        }

        #endregion

        #region --- Private helpers ---

        private bool HasValidPassword()
        {
            bool isPasswordValid = true;
            if (!Validator.HasValidPassword(Password))
            {
                PasswordWrongText = Resource["HasValidPassword"];
                isPasswordValid = false;
            }
            else
            {
                PasswordWrongText = string.Empty;
            }
            return isPasswordValid;
        }

        private bool HasValidConfirmPassword()
        {
            bool isPasswordValid = true;
            if (!Validator.HasEqualPasswords(Password, ConfirmPassword))
            {
                ConfirmPasswordWrongText = Resource["HasMatchPasswords"];
                isPasswordValid = false;
            }
            else
            {
                ConfirmPasswordWrongText = string.Empty;
            }
            return isPasswordValid;
        }

        private UserModel CreateUser()
        {
            var user = _userModel;
            if (!Password.Equals(_userModel.Name))
            {
                PasswordWrongText = string.Empty;
                user.Password = Password;
            }
            else
            {
                user = null;
                PasswordWrongText = Resource["HasConsidePasswordWithName"];
            }

            return user;
        }

        private async void OnGoBackTapAsync()
        {
            await NavigationService.GoBackAsync();
        }

        private void OnPasswordVisibleTap()
        {
            IsPassword = !IsPassword;
        }

        private void OnConfirmPasswordVisibleTap()
        {
            IsConfirmPassword = !IsConfirmPassword;
        }

        private async void OnCreateAccountTapAsync()
        {
            if (HasValidPassword() & HasValidConfirmPassword())
            {
                var user = CreateUser();
                if (user != null)
                {
                    var parameters = new NavigationParameters();
                    await _authorizationService.CreateAccountAsync(_userModel);
                    parameters.Add(nameof(UserModel), _userModel);
                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(WelcomePage)}/{nameof(LogInPage)}", parameters);
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
