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

        private ICommand _englishLanguageTapCommand;
        public ICommand EnglishLanguageTapCommand =>
            _englishLanguageTapCommand ??= new DelegateCommand(OnEnglishLanguageTap);

        private ICommand _russianLanguageTapCommand;
        public ICommand RussianLanguageTapCommand =>
            _russianLanguageTapCommand ??= new DelegateCommand(OnRussianLanguageTap);

        #endregion

        #region --- Overrides ---

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            SelectedLanguage = Resource.Lang;
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

        private void OnEnglishLanguageTap()
        {
            SelectedLanguage = Constants.ENGLISH_LANGUAGE;
        }

        private void OnRussianLanguageTap()
        {
            SelectedLanguage = Constants.RUSSIAN_LANGUAGE;
        }

        #endregion

    }
}
