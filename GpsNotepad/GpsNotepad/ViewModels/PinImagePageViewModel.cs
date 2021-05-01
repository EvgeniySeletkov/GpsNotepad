using GpsNotepad.Models;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.PinImage;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GpsNotepad.ViewModels
{
    class PinImagePageViewModel : BaseViewModel
    {
        private readonly IPinImageService _pinImageService;

        public PinImagePageViewModel(INavigationService navigationService, 
                                      ILocalizationService localizationService,
                                      IPinImageService pinImageService) : base(navigationService, localizationService)
        {
            _pinImageService = pinImageService;
        }

        private ObservableCollection<PinImageModel> _imageList;
        public ObservableCollection<PinImageModel> ImageList
        {
            get => _imageList;
            set => SetProperty(ref _imageList, value);
        }

        private int _imagePosition;
        public int ImagePosition
        {
            get => _imagePosition;
            set => SetProperty(ref _imagePosition, value);
        }

        private string _imagePositionLabel;
        public string ImagePositionLabel
        {
            get => _imagePositionLabel;
            set => SetProperty(ref _imagePositionLabel, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<ObservableCollection<PinImageModel>>(nameof(ImageList), out var imageList))
            {
                ImageList = new ObservableCollection<PinImageModel>(imageList);
            }
            if (parameters.TryGetValue<PinImageModel>(nameof(PinImageModel), out var selectedImage))
            {
                ImagePosition = ImageList.IndexOf(selectedImage);
                ImagePositionLabel = $"{ImagePosition + 1} - {ImageList.Count}";
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(ImagePosition))
            {
                ImagePositionLabel = $"{ImagePosition + 1} - {ImageList.Count}";
            }
        }

    }
}
