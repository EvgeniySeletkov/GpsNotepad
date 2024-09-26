using GpsNotepad.Services.Localization;
using GpsNotepad.Services.ThemeService;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModels
{
    class SettingsPageViewModel : BaseViewModel
    {
        private readonly IThemeService _themeService;

        public SettingsPageViewModel(INavigationService navigationService,
                                     ILocalizationService localizationService,
                                     IThemeService themeService) : base(navigationService, localizationService)
        {
            _themeService = themeService;
        }

        #region --- Public properties ---

        private bool _darkTheme;
        public bool DarkTheme
        {
            get => _darkTheme;
            set => SetProperty(ref _darkTheme, value);
        }

        private string selectedTheme;
        public string SelectedTheme
        {
            get => selectedTheme;
            set => SetProperty(ref selectedTheme, value);
        }

        private string selectedLang;
        public string SelectedLang
        {
            get => selectedLang;
            set => SetProperty(ref selectedLang, value);
        }

        private ICommand _goBackTapCommand;
        public ICommand GoBackTapCommand =>
            _goBackTapCommand ??= new DelegateCommand(OnGoBackTapAsync);

        private ICommand _openLanguageSettingsTapCommand;
        public ICommand OpenLanguageSettingsTapCommand =>
            _openLanguageSettingsTapCommand ??= new DelegateCommand(OnOpenLanguageSettingsTapAsync);

        #endregion

        #region --- Overrides ---

        public override void Initialize(INavigationParameters parameters)
        {
            var theme = _themeService.GetTheme();
            DarkTheme = theme == OSAppTheme.Dark.ToString();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(DarkTheme))
            {
                var theme = DarkTheme ? OSAppTheme.Dark : OSAppTheme.Light;
                _themeService.SetTheme(theme);
            }
        }

        #endregion

        #region --- Private helpers ---

        private async void OnGoBackTapAsync()
        {
            await NavigationService.GoBackAsync();
        }

        private async void OnOpenLanguageSettingsTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(LanguageSettingsPage));
        }

        #endregion
    }
}
