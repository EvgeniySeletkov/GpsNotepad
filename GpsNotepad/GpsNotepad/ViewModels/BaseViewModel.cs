using GpsNotepad.Services.Localization;
using Prism.Mvvm;
using Prism.Navigation;

namespace GpsNotepad.ViewModels
{
    class BaseViewModel : BindableBase, IInitialize, INavigationAware
    {
        protected INavigationService NavigationService { get; private set; }
        public ILocalizationService Resource{ get; set; }

        public BaseViewModel(INavigationService navigationService,
                              ILocalizationService localizationService)
        {
            NavigationService = navigationService;
            Resource = localizationService;
        }

        #region --- IterfaceName implementation ---

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #endregion

    }
}
