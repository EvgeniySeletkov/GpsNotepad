using GpsNotepad.Services.Localization;
using Prism.Navigation;

namespace GpsNotepad.ViewModels
{
    class MainMapTabbedPageViewModel : BaseViewModel
    {
        // TODO: Add selected tab background color.
        public MainMapTabbedPageViewModel(INavigationService navigationService,
                                          ILocalizationService localizationService) : base(navigationService, localizationService)
        {

        }

    }
}
