using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Settings;
using GpsNotepad.ViewModels;
using GpsNotepad.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad
{
    public partial class App : PrismApplication
    {
        public App() { }

        private ILocalizationService _localizationService;
        private ILocalizationService LocalizationService =>
            _localizationService ??= Container.Resolve<ILocalizationService>();

        private IAuthorizationService _authorizationService;
        private IAuthorizationService AuthorizationService =>
            _authorizationService ??= Container.Resolve<IAuthorizationService>();
        

        #region --- Overrides ---

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Services
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<ILocalizationService>(Container.Resolve<LocalizationService>());

            // Navigations
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<MainMapPage, MainMapPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage>();
            containerRegistry.RegisterForNavigation<PinsListPage>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
        }

        protected async override void OnInitialized()
        {
            InitializeComponent();

            LocalizationService.SetLocalization();

            //if (AuthorizationService.IsAuthorized)
            //{
            //    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapPage)}");
            //}
            //else
            //{
            //    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
            //}

            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapPage)}");

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
