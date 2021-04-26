using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MediaService;
using GpsNotepad.Services.Pin;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class AddEditPageViewModel : BaseViewModel
    {
        private PinViewModel _pinViewModel;

        private readonly IPinService _pinService;
        private readonly IMediaService _mediaService;

        public AddEditPageViewModel(INavigationService navigationService,
                                    ILocalizationService localizationService,
                                    IMediaService mediaService,
                                    IPinService pinService) : base(navigationService, localizationService)
        {
            _mediaService = mediaService;
            _pinService = pinService;
            title = "Add Pin";
            pinViewModelList = new ObservableCollection<PinViewModel>();
            latitideEntry = "0";
            longitideEntry = "0";

            PinImageList = new ObservableCollection<PinImageModel>();
        }

        #region --- Public properties ---

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private ObservableCollection<PinViewModel> pinViewModelList;
        public ObservableCollection<PinViewModel> PinViewModelList
        {
            get => pinViewModelList;
            set => SetProperty(ref pinViewModelList, value);
        }

        private ObservableCollection<PinImageModel> pinImageList;
        public ObservableCollection<PinImageModel> PinImageList
        {
            get => pinImageList;
            set => SetProperty(ref pinImageList, value);
        }

        private double pinImageListHeight;
        public double PinImageListHeight
        {
            get => pinImageListHeight;
            set => SetProperty(ref pinImageListHeight, value);
        }

        private MapSpan cameraPosition;
        public MapSpan CameraPosition
        {
            get => cameraPosition;
            set => SetProperty(ref cameraPosition, value);
        }

        private string labelEntry;
        public string LabelEntry
        {
            get => labelEntry;
            set => SetProperty(ref labelEntry, value);
        }

        private string addressEntry;
        public string AddressEntry
        {
            get => addressEntry;
            set => SetProperty(ref addressEntry, value);
        }

        private string latitideEntry;
        public string LatitudeEntry
        {
            get => latitideEntry;
            set => SetProperty(ref latitideEntry, value);
        }

        private string longitideEntry;
        public string LongitudeEntry
        {
            get => longitideEntry;
            set => SetProperty(ref longitideEntry, value);
        }

        private string descriptionEntry;
        public string DescriptionEntry
        {
            get => descriptionEntry;
            set => SetProperty(ref descriptionEntry, value);
        }

        private ICommand mapTapCommand;
        public ICommand MapTapCommand =>
            mapTapCommand ??= new DelegateCommand<object>(OnMapTap);

        private ICommand addImageTapCommand;
        public ICommand AddImageTapCommand =>
            addImageTapCommand ??= new DelegateCommand(OnAddImageTap);

        private ICommand savePinTapCommand;
        public ICommand SavePinTapCommand =>
            savePinTapCommand ??= new DelegateCommand(OnSavePinTap);

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(nameof(PinViewModel), out var pinViewModel))
            {
                Title = "Edit Pin";
                _pinViewModel = pinViewModel;
                LabelEntry = pinViewModel.Label;
                AddressEntry = pinViewModel.Address;
                LatitudeEntry = pinViewModel.Latitude.ToString();
                LongitudeEntry = pinViewModel.Longitude.ToString();
                DescriptionEntry = pinViewModel.Description;
                PinViewModelList = new ObservableCollection<PinViewModel>();
                PinViewModelList.Add(_pinViewModel);
                var postion = new Position(pinViewModel.Latitude, pinViewModel.Longitude);
                CameraPosition = new MapSpan(postion, 1, 1);
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if ((args.PropertyName == nameof(LabelEntry) ||
                args.PropertyName == nameof(LatitudeEntry) ||
                args.PropertyName == nameof(LongitudeEntry) ||
                args.PropertyName == nameof(DescriptionEntry)) &&
                !HasEmptyCordinates())
            {
                bool isLatitude = double.TryParse(LatitudeEntry, out var latitude);
                bool isLongitude = double.TryParse(LongitudeEntry, out var longitude);
                if (isLatitude && isLongitude)
                {
                    var position = new Position(latitude, longitude);
                    await AddPinViewModelOnMapAsync(position);
                }
            }
            if (args.PropertyName == nameof(PinImageList))
            {
                PinImageListHeight = PinImageList.Count < 3 ? PinImageList.Count * 42 : 84;
            }
        }

        #endregion

        #region --- Private helpers ---

        private bool HasEmptyCordinates()
        {
            bool isEmpty = false;
            if (string.IsNullOrWhiteSpace(LatitudeEntry) ||
                string.IsNullOrWhiteSpace(LongitudeEntry))
            {
                isEmpty = true;
            }

            return isEmpty;
        }

        private void CreatePinImageModel(string imagePath)
        {
            if (imagePath != null)
            {
                var imageList = new List<PinImageModel>(PinImageList);
                var pinImageModel = new PinImageModel()
                {
                    Image = imagePath
                };
                imageList.Add(pinImageModel);
                PinImageList = new ObservableCollection<PinImageModel>(imageList);
            }
        }

        private async Task CreatePinAsync(Position position)
        {
            var address = await _pinService.GetAddressAsync(position);

            _pinViewModel = new PinViewModel()
            {
                Label = !string.IsNullOrWhiteSpace(LabelEntry) ? LabelEntry : "Untitled pin",
                Latitude = Math.Round(position.Latitude, 2),
                Longitude = Math.Round(position.Longitude, 2),
                Address = address,
                IsVisible = true,
                Description = DescriptionEntry
            };
        }

        private async Task EditPinAsync(Position position)
        {
            var address = await _pinService.GetAddressAsync(position);
            _pinViewModel.Label = !string.IsNullOrWhiteSpace(LabelEntry) ? LabelEntry : "Untitled pin";
            _pinViewModel.Latitude = Math.Round(position.Latitude, 2);
            _pinViewModel.Longitude = Math.Round(position.Longitude, 2);
            _pinViewModel.Address = address;
            _pinViewModel.Description = DescriptionEntry;

        }

        private async Task AddPinViewModelOnMapAsync(Position position)
        {
            if (_pinViewModel == null)
            {
                await CreatePinAsync(position);

            }
            else
            {
                await EditPinAsync(position);
            }

            var pinViewModelList = new List<PinViewModel>();
            pinViewModelList.Add(_pinViewModel);
            PinViewModelList = new ObservableCollection<PinViewModel>(pinViewModelList);
        }

        private async void OnMapTap(object obj)
        {
            var position = (Position)obj;
            await AddPinViewModelOnMapAsync(position);
            LabelEntry = _pinViewModel.Label;
            AddressEntry = _pinViewModel.Address;
            LatitudeEntry = _pinViewModel.Latitude.ToString();
            LongitudeEntry = _pinViewModel.Longitude.ToString();
        }

        private void OnAddImageTap()
        {
            var config = new ActionSheetConfig
            {
                Title = "Alert"
            };

            config.Add("Gallery", new Action(async () =>
            {
                string imagePath = await _mediaService.TakePhotoFromGalleryAsync();
                CreatePinImageModel(imagePath);
            }), icon: "ic_collections_black.png");

            config.Add("Camera", new Action(async () =>
            {
                string imagePath = await _mediaService.TakePhotoWithCameraAsync();
                CreatePinImageModel(imagePath);
            }), icon: "ic_camera_alt_black.png");

            UserDialogs.Instance.ActionSheet(config);
        }

        private async void OnSavePinTap()
        {
            if (!string.IsNullOrWhiteSpace(LabelEntry) && !HasEmptyCordinates())
            {
                var pinModels = await _pinService.GetAllPinsAsync();
                var uniquePinModel = pinModels.FirstOrDefault(p => p.Label == LabelEntry);
                if (uniquePinModel == null && _pinViewModel != null)
                {
                    var pinModel = _pinViewModel.ToPinModel();
                    if (_pinViewModel.PinId == 0)
                    {
                        await _pinService.InsertPinAsync(pinModel);
                    }
                    else
                    {
                        await _pinService.UpdatePinAsync(pinModel);
                    }
                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                }
                else
                {
                    //UserDialogs.Instance.Alert("Latitude or Longitude field is not digit!", Resource["Alert"], "OK");
                    UserDialogs.Instance.Alert("Name must be unique.", Resource["Alert"], "OK");
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Name, Latitude or Longitude field is empty.", Resource["Alert"], "OK");
            }
        }

        #endregion

    }
}
