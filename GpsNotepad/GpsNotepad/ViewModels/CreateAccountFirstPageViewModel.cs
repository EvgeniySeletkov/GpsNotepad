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
    class CreateAccountFirstPageViewModel : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public CreateAccountFirstPageViewModel(IAuthorizationService authorizationService,
                                               ILocalizationService localizationService,
                                               INavigationService navigationService) : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
        }

        #region --- Public Properties ---

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _nameWrongText;
        public string NameWrongText
        {
            get => _nameWrongText;
            set => SetProperty(ref _nameWrongText, value);
        }

        private bool _isNameWrongVisible;
        public bool IsNameWrongVisible
        {
            get => _isNameWrongVisible;
            set => SetProperty(ref _isNameWrongVisible, value);
        }

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

        private bool _isEmailWrongVisible;
        public bool IsEmailWrongVisible
        {
            get => _isEmailWrongVisible;
            set => SetProperty(ref _isEmailWrongVisible, value);
        }

        private bool _isNextButtonEnabled;
        public bool IsNextButtonEnabled
        {
            get => _isNextButtonEnabled;
            set => SetProperty(ref _isNextButtonEnabled, value);
        }

        private ICommand _nameClearTapCommand;
        public ICommand NameClearTapCommand =>
            _nameClearTapCommand ??= new DelegateCommand(OnNameClearTap);

        private ICommand _emailClearTapCommand;
        public ICommand EmailClearTapCommand =>
            _emailClearTapCommand ??= new DelegateCommand(OnEmailClearTap);

        private ICommand _nextTapCommand;
        public ICommand NextTapCommand => 
            _nextTapCommand ??= new DelegateCommand(OnNextTapAsync);

        private ICommand _logInWithFacebookTapCommand;
        public ICommand LogInWithFacebookTapCommand =>
            _logInWithFacebookTapCommand ??= new DelegateCommand(OnLogInWithFacebookTapAsync);


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

        #region --- Private helpers ---

        private bool HasValidName()
        {
            bool isNameValid;
            if (!Validator.HasValidName(Name))
            {
                IsNameWrongVisible = true;
                NameWrongText = Resource["HasValidName"];
                isNameValid = false;
            }
            else
            {
                IsNameWrongVisible = false;
                isNameValid = true;
            }
            return isNameValid;
        }

        private bool HasValidEmail()
        {
            bool isEmailValid;
            if (!Validator.HasValidEmail(Email))
            {
                IsEmailWrongVisible = true;
                EmailWrongText = Resource["HasValidEmail"];
                isEmailValid = false;
            }
            else
            {
                IsEmailWrongVisible = false;
                isEmailValid = true;
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

        private void OnNameClearTap()
        {
            Name = string.Empty;
        }

        private void OnEmailClearTap()
        {
            Email = string.Empty;
        }

        private async void OnNextTapAsync()
        {
            if (HasValidName() &&
                HasValidEmail())
            {
                var userModel = CreateUser();
                if (userModel != null)
                {
                    var isBusyEmail = await _authorizationService.HasEmailAsync(Email);
                    if (!isBusyEmail)
                    {
                        IsEmailWrongVisible = false;
                        var parameters = new NavigationParameters();
                        parameters.Add(nameof(UserModel), userModel);
                        await NavigationService.NavigateAsync(nameof(CreateAccountSecondPage), parameters);
                    }
                    else
                    {
                        IsEmailWrongVisible = true;
                        EmailWrongText = Resource["HasBusyEmail"];
                    }
                }
            }
        }

        private async void OnLogInWithFacebookTapAsync()
        {
            var response = await _authorizationService.LogInWithFacebookAsync();
            switch (response.Status)
            {
                case FacebookActionStatus.Completed:
                    bool isAutorized = await _authorizationService.AuthorizeWithFacebookAsync();
                    if (isAutorized)
                    {
                        await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                    }
                    break;
                case FacebookActionStatus.Canceled:
                    break;
                case FacebookActionStatus.Error:
                    break;
                case FacebookActionStatus.Unauthorized:
                    break;
            }
        }

        #endregion

    }
}
