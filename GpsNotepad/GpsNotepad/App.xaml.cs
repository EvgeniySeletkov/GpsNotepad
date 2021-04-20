using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MapCameraPosition;
using GpsNotepad.Services.Permission;
using GpsNotepad.Services.Pin;
using GpsNotepad.Services.PinImage;
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
            containerRegistry.RegisterInstance<IPermissionService>(Container.Resolve<PermissionService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<ILocalizationService>(Container.Resolve<LocalizationService>());
            containerRegistry.RegisterInstance<IMapCameraPositionService>(Container.Resolve<MapCameraPositionService>());
            containerRegistry.RegisterInstance<IPinImageService>(Container.Resolve<PinImageService>());

            // Navigations
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<MainMapTabbedPage, MainMapTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<MapTabPage, MapTabPageViewModel>();
            containerRegistry.RegisterForNavigation<PinsListTabPage, PinsListTabPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPage, AddEditPageViewModel>();
            containerRegistry.RegisterForNavigation<PinImagesPage, PinImagesPageViewModel>();
            containerRegistry.RegisterForNavigation<LogInAndRegisterPage, LogInAndRegisterPageViewModel>();
        }

        protected async override void OnInitialized()
        {
            InitializeComponent();



            if (AuthorizationService.IsAuthorized)
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                //await NavigationService.NavigateAsync($"/{nameof(MainMapTabbedPage)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
            }

            //await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LogInAndRegisterPage)}");

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
