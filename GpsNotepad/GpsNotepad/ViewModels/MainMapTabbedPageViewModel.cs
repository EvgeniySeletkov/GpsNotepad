using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Settings;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

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
