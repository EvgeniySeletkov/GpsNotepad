using GpsNotepad.Services.Localization;
using Prism.Commands;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;

namespace GpsNotepad.ViewModels
{
    class LanguageSettingsPageViewModel : BaseViewModel
    {

        public LanguageSettingsPageViewModel(INavigationService navigationService,
                                             ILocalizationService localizationService) : base(navigationService, localizationService)
        {
        }

        #region --- Public properties ---

        private object _selectedLanguage;

        public object SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        private ICommand _goBackTapCommand;
        public ICommand GoBackTapCommand =>
            _goBackTapCommand ??= new DelegateCommand(OnGoBackTapAsync);

        #endregion

        #region --- Overrides ---

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            switch (Resource.Lang)
            {
                case Constants.ENGLISH_LANGUAGE:
                    SelectedLanguage = Constants.ENGLISH_LANGUAGE;
                    break;
                case Constants.RUSSIAN_LANGUAGE:
                    SelectedLanguage = Constants.RUSSIAN_LANGUAGE;
                    break;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedLanguage))
            {
                Resource.Lang = SelectedLanguage.ToString();
                Resource.SetCulture(SelectedLanguage.ToString());
            }
        }

        #endregion

        #region --- Private helpers ---

        private async void OnGoBackTapAsync()
        {
            await NavigationService.GoBackAsync();
        }

        #endregion

    }
}
