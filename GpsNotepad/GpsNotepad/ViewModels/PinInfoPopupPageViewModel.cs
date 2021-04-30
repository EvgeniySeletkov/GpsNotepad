using GpsNotepad.Models;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.PinImage;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;

namespace GpsNotepad.ViewModels
{
    class PinInfoPopupPageViewModel : BaseViewModel
    {
        private readonly IPinImageService _pinImageService;

        public PinInfoPopupPageViewModel(INavigationService navigationService,
                                         ILocalizationService localizationService,
                                         IPinImageService pinImageService) : base(navigationService, localizationService)
        {
            _pinImageService = pinImageService;
        }

        #region --- Public properties ---

        private double _imageWidth;
        public double ImageWidth
        {
            get => _imageWidth;
            set => SetProperty(ref _imageWidth, value);
        }

        private ObservableCollection<PinImageModel> _imageList;
        public ObservableCollection<PinImageModel> ImageList
        {
            get => _imageList;
            set => SetProperty(ref _imageList, value);
        }

        private bool _areImagesVisible;
        public bool AreImagesVisible
        {
            get => _areImagesVisible;
            set => SetProperty(ref _areImagesVisible, value);
        }

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

        //TODO
        private ICommand _openImagePageTapCommand;
        public ICommand OpenImagePageTapCommand =>
            _openImagePageTapCommand ??= new DelegateCommand<PinImageModel>(OnOpenImagePageTap);

        private void OnOpenImagePageTap(PinImageModel im)
        {
            System.Console.WriteLine();
        }

        private ICommand _closePopupPageTapCommand;
        public ICommand ClosePopupPageTapCommand =>
            _closePopupPageTapCommand ??= new DelegateCommand(OnClosePopupPageTapAsync);

        #endregion

        #region --- Overrides ---

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(nameof(PinViewModel), out var selectedPin))
            {
                var imageList = await _pinImageService.GetAllPinImagesAsync(selectedPin.PinId);
                AreImagesVisible = imageList.Count > 0;
                ImageList = new ObservableCollection<PinImageModel>(imageList);

                Label = selectedPin.Label;
                Coordinates = selectedPin.Coordinates;
                Description = selectedPin.Description;

                // TODO
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                var width = mainDisplayInfo.Width / mainDisplayInfo.Density / 3;
                ImageWidth = width;
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
