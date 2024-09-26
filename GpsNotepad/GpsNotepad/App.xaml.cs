using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MapCameraPosition;
using GpsNotepad.Services.MediaService;
using GpsNotepad.Services.Permission;
using GpsNotepad.Services.Pin;
using GpsNotepad.Services.PinImage;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Settings;
using GpsNotepad.Services.ThemeService;
using GpsNotepad.ViewModels;
using GpsNotepad.Views;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GpsNotepad
{
    public partial class App : PrismApplication
    {
        public App() { }

        private IAuthorizationService _authorizationService;
        private IAuthorizationService AuthorizationService =>
            _authorizationService ??= Container.Resolve<IAuthorizationService>();

        private IThemeService _themeService;
        private IThemeService ThemeService =>
            _themeService ??= Container.Resolve<IThemeService>();


        #region --- Overrides ---

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Services
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IThemeService>(Container.Resolve<ThemeService>());
            containerRegistry.RegisterInstance<ILocalizationService>(Container.Resolve<LocalizationService>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IPermissionService>(Container.Resolve<PermissionService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<IMapCameraPositionService>(Container.Resolve<MapCameraPositionService>());
            containerRegistry.RegisterInstance<IMediaService>(Container.Resolve<MediaService>());
            containerRegistry.RegisterInstance<IPinImageService>(Container.Resolve<PinImageService>());

            // Navigations
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LogInPage, LogInPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateAccountFirstPage, CreateAccountFirstPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateAccountSecondPage, CreateAccountSecondPageViewModel>();
            containerRegistry.RegisterForNavigation<MainMapTabbedPage, MainMapTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<MapTabPage, MapTabPageViewModel>();
            containerRegistry.RegisterForNavigation<PinListTabPage, PinListTabPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPage, AddEditPageViewModel>();
            containerRegistry.RegisterForNavigation<PinImagePage, PinImagePageViewModel>();
            containerRegistry.RegisterForNavigation<WelcomePage, WelcomePageViewModel>();
            containerRegistry.RegisterForNavigation<PinInfoPopupPage, PinInfoPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<LanguageSettingsPage, LanguageSettingsPageViewModel>();
        }

        protected async override void OnInitialized()
        {
            InitializeComponent();

            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var imageWidth = displayInfo.Width / displayInfo.Density / 3;

            Resources.Add(nameof(imageWidth), imageWidth);

            Application.Current.UserAppTheme = (OSAppTheme)Enum.Parse(typeof(OSAppTheme), ThemeService.GetTheme());

            if (AuthorizationService.IsAuthorized)
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(WelcomePage)}");
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion
    }
}
