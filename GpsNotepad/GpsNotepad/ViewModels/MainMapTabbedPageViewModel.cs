using GpsNotepad.Services.Localization;
using Prism.Navigation;

namespace GpsNotepad.ViewModels
{
    class MainMapTabbedPageViewModel : BaseViewModel
    {
        public MainMapTabbedPageViewModel(INavigationService navigationService,
                                          ILocalizationService localizationService) : base(navigationService, localizationService)
        {
        }

    }
}
