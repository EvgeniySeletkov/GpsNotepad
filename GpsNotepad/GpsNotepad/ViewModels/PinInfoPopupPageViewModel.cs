using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Localization;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace GpsNotepad.ViewModels
{
    class PinInfoPopupPageViewModel : BaseViewModel
    {
        public PinInfoPopupPageViewModel(INavigationService navigationService,
                                         ILocalizationService localizationService) : base(navigationService, localizationService)
        {

        }

        #region --- Public properties ---

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private string _coordinates;
        public string Coordinates
        {
            get => _coordinates;
            set => SetProperty(ref _coordinates, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private ICommand _closePopupPageTapCommand;
        public ICommand ClosePopupPageTapCommand =>
            _closePopupPageTapCommand ??= new DelegateCommand(OnClosePopupPageTapAsync);

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(nameof(PinViewModel), out var selectedPin))
            {
                Label = selectedPin.Label;
                Coordinates = selectedPin.Coordinates;
                Description = selectedPin.Description;
            }
        }

        #endregion

        #region --- Private helpers ---

        private async void OnClosePopupPageTapAsync()
        {
            await NavigationService.GoBackAsync();
        }

        #endregion

    }
}
