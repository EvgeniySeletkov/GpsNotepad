using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MediaService;
using GpsNotepad.Services.Permission;
using GpsNotepad.Services.Pin;
using GpsNotepad.Services.PinImage;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class AddEditPageViewModel : BaseViewModel
    {
        private PinViewModel _pinViewModel;

        private readonly IPinService _pinService;
        private readonly IPermissionService _permissionService;
        private readonly IMediaService _mediaService;
        private readonly IPinImageService _pinImageService;

        public AddEditPageViewModel(INavigationService navigationService,
                                    ILocalizationService localizationService,
                                    IPinService pinService,
                                    IPermissionService permissionService,
                                    IMediaService mediaService,
                                    IPinImageService pinImageService
                                    ) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _permissionService = permissionService;
            _mediaService = mediaService;
            _pinImageService = pinImageService;
            
            _title = Resource["Add pin"];
            PinList = new ObservableCollection<PinViewModel>();
            PinImageList = new ObservableCollection<PinImageModel>();
        }

        #region --- Public properties ---

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isMyLocation;
        public bool IsMyLocation
        {
            get => _isMyLocation;
            set => SetProperty(ref _isMyLocation, value);
        }

        private ObservableCollection<PinViewModel> _pinList;
        public ObservableCollection<PinViewModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
        }

        private MapSpan _cameraPosition;
        public MapSpan CameraPosition
        {
            get => _cameraPosition;
            set => SetProperty(ref _cameraPosition, value);
        }

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private string _labelWrongText;
        public string LabelWrongText
        {
            get => _labelWrongText;
            set => SetProperty(ref _labelWrongText, value);
        }

        private bool _isLabelWrongVisible;
        public bool IsLabelWrongVisible
        {
            get => _isLabelWrongVisible;
            set => SetProperty(ref _isLabelWrongVisible, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _descriptionWrongText;
        public string DescriptionWrongText
        {
            get => _descriptionWrongText;
            set => SetProperty(ref _descriptionWrongText, value);
        }

        private bool _isDescriptionWrongVisible;
        public bool IsDescriptionWrongVisible
        {
            get => _isDescriptionWrongVisible;
            set => SetProperty(ref _isDescriptionWrongVisible, value);
        }

        private string _latitide;
        public string Latitude
        {
            get => _latitide;
            set => SetProperty(ref _latitide, value);
        }

        private string _latitudeWrongText;
        public string LatitudeWrongText
        {
            get => _latitudeWrongText;
            set => SetProperty(ref _latitudeWrongText, value);
        }

        private bool _isLatitudeWrongVisible;
        public bool IsLatitudeWrongVisible
        {
            get => _isLatitudeWrongVisible;
            set => SetProperty(ref _isLatitudeWrongVisible, value);
        }

        private string _longitide;
        public string Longitude
        {
            get => _longitide;
            set => SetProperty(ref _longitide, value);
        }

        private string _longitudeWrongText;
        public string LongitudeWrongText
        {
            get => _longitudeWrongText;
            set => SetProperty(ref _longitudeWrongText, value);
        }

        private bool _isLongitudeWrongVisible;
        public bool IsLongitudeWrongVisible
        {
            get => _isLongitudeWrongVisible;
            set => SetProperty(ref _isLongitudeWrongVisible, value);
        }

        private ObservableCollection<PinImageModel> _pinImageList;
        public ObservableCollection<PinImageModel> PinImageList
        {
            get => _pinImageList;
            set => SetProperty(ref _pinImageList, value);
        }

        private double _pinImageListHeight;
        public double PinImageListHeight
        {
            get => _pinImageListHeight;
            set => SetProperty(ref _pinImageListHeight, value);
        }

        private ICommand _goBackTapCommand;
        public ICommand GoBackTapCommand =>
            _goBackTapCommand ??= new DelegateCommand(OnGoBackTapAsync);

        private ICommand _savePinTapCommand;
        public ICommand SavePinTapCommand =>
            _savePinTapCommand ??= new DelegateCommand(OnSavePinTapAsync);

        private ICommand _mapTapCommand;
        public ICommand MapTapCommand =>
            _mapTapCommand ??= new DelegateCommand<object>(OnMapTapAsync);

        private ICommand _clearLabelTapCommand;
        public ICommand ClearLabelTapCommand =>
            _clearLabelTapCommand ??= new DelegateCommand(OnClearLabelTap);

        private ICommand _clearDescriptionTapCommand;
        public ICommand ClearDescriptionTapCommand =>
            _clearDescriptionTapCommand ??= new DelegateCommand(OnClearDescriptionTap);

        private ICommand _clearLatitudeTapCommand;
        public ICommand ClearLatitudeTapCommand =>
            _clearLatitudeTapCommand ??= new DelegateCommand(OnClearLatitudeTap);

        private ICommand _clearLongitudeTapCommand;
        public ICommand ClearLongitudeTapCommand =>
            _clearLongitudeTapCommand ??= new DelegateCommand(OnClearLongitudeTap);

        private ICommand _addImageTapCommand;
        public ICommand AddImageTapCommand =>
            _addImageTapCommand ??= new DelegateCommand(OnAddImageTap);

        private ICommand _deleteImageTapCommand;
        public ICommand DeleteImageTapCommand =>
            _deleteImageTapCommand ??= new DelegateCommand<PinImageModel>(OnDeleteImageTapAsync);

        #endregion

        #region --- Overrides ---

        public override async void Initialize(INavigationParameters parameters)
        {
            var status = await _permissionService.RequestLocationPermissionAsync();

            IsMyLocation = status == PermissionStatus.Granted;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(nameof(PinViewModel), out var pinViewModel))
            {
                Title = Resource["Edit pin"];
                _pinViewModel = pinViewModel;
                Label = pinViewModel.Label;
                Latitude = pinViewModel.Latitude.ToString();
                Longitude = pinViewModel.Longitude.ToString();
                Description = pinViewModel.Description;
                PinList = new ObservableCollection<PinViewModel>();
                PinList.Add(_pinViewModel);
                var position = new Position(pinViewModel.Latitude, pinViewModel.Longitude);
                CameraPosition = new MapSpan(position, 1, 1);
                var pinImageList = await _pinImageService.GetAllPinImagesAsync(_pinViewModel.PinId);
                PinImageList = new ObservableCollection<PinImageModel>(pinImageList);
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Latitude) ||
                args.PropertyName == nameof(Longitude))
            {
                bool isLatitude = double.TryParse(Latitude, out var latitude);
                bool isLongitude = double.TryParse(Longitude, out var longitude);
                Latitude = isLatitude ? Latitude : string.Empty;
                Longitude = isLongitude ? Longitude : string.Empty;

                if (!HasEmptyLatitude() && !HasEmptyLongitude())
                {
                    var position = new Position(latitude, longitude);
                    CameraPosition = new MapSpan(position, 1, 1);
                    await AddPinViewModelOnMapAsync(position);
                }
            }
            if (args.PropertyName == nameof(PinImageList))
            {
                ChangeImageListHeight();
            }
        }

        #endregion

        #region --- Private helpers ---

        private bool HasEmptyLabel()
        {
            bool isEmpty;
            if (string.IsNullOrWhiteSpace(Label))
            {
                isEmpty = true;
                LabelWrongText = Resource["Label is empty"];
            }
            else
            {
                isEmpty = false;
                LabelWrongText = string.Empty;
            }

            return isEmpty;
        }

        private bool HasEmptyLatitude()
        {
            bool isEmpty;
            if (string.IsNullOrWhiteSpace(Latitude))
            {
                isEmpty = true;
                LatitudeWrongText = Resource["Latitude is empty"];
            }
            else
            {
                isEmpty = false;
                LatitudeWrongText = string.Empty;
            }

            return isEmpty;
        }

        private bool HasEmptyLongitude()
        {
            bool isEmpty;
            if (string.IsNullOrWhiteSpace(Longitude))
            {
                isEmpty = true;
                LongitudeWrongText = Resource["Longitude is empty"];
            }
            else
            {
                isEmpty = false;
                LongitudeWrongText = string.Empty;
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
                    ImagePath = imagePath
                };
                imageList.Add(pinImageModel);
                PinImageList = new ObservableCollection<PinImageModel>(imageList);
            }
        }

        private void ChangeImageListHeight()
        {
            PinImageListHeight = PinImageList.Count < 3 ? PinImageList.Count * 42 : 84;
        }

        private async Task CreatePinAsync(Position position)
        {
            var address = await _pinService.GetAddressAsync(position);

            _pinViewModel = new PinViewModel()
            {
                Label = !string.IsNullOrWhiteSpace(Label) ? Label : Resource["Untitled pin"],
                Latitude = Math.Round(position.Latitude, 8),
                Longitude = Math.Round(position.Longitude, 8),
                Address = address,
                IsFavorite = true
            };
        }

        private async Task EditPinAsync(Position position)
        {
            var address = await _pinService.GetAddressAsync(position);
            _pinViewModel.Label = !string.IsNullOrWhiteSpace(Label) ? Label : Resource["Untitled pin"];
            _pinViewModel.Latitude = Math.Round(position.Latitude, 8);
            _pinViewModel.Longitude = Math.Round(position.Longitude, 8);
            _pinViewModel.Address = address;

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
            PinList = new ObservableCollection<PinViewModel>(pinViewModelList);
        }

        private async Task SaveImagesAsync()
        {
            var pinList = await _pinService.GetAllPinsAsync();
            var pin = pinList.FirstOrDefault(pin => pin.Label == Label);

            if (pin != null)
            {
                var pinImageList = await _pinImageService.GetAllPinImagesAsync(pin.Id);
                foreach (var pinImage in PinImageList)
                {
                    if (pinImageList.FirstOrDefault(image => image.ImagePath == pinImage.ImagePath) == null)
                    {
                        pinImage.PinId = pin.Id;
                        await _pinImageService.SavePinImageAsync(pinImage);
                    }
                }
            }
            
        }

        private async void OnGoBackTapAsync()
        {
            await NavigationService.GoBackAsync();
        }

        private async void OnSavePinTapAsync()
        {
            if (!HasEmptyLabel() &&
                !HasEmptyLatitude() &&
                !HasEmptyLongitude())
            {
                var pinModels = await _pinService.GetAllPinsAsync();
                _pinViewModel.Label = Label;
                _pinViewModel.Description = Description;
                var uniquePin = pinModels.FirstOrDefault(p => p.Label == Label);
                var pin = _pinViewModel.ToPinModel(); ;

                if (uniquePin == null)
                {
                    IsLabelWrongVisible = false;
                    if (_pinViewModel.PinId == 0)
                    {
                        await _pinService.InsertPinAsync(pin);
                        await SaveImagesAsync();
                    }
                    else
                    {
                        await _pinService.UpdatePinAsync(pin);
                        await SaveImagesAsync();
                    }

                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                }
                else
                {
                    if (_pinViewModel.PinId != uniquePin.Id)
                    {
                        LabelWrongText = Resource["NotUniqueLabel"];
                        IsLabelWrongVisible = true;
                    }
                    else
                    {
                        IsLabelWrongVisible = false;
                        await _pinService.UpdatePinAsync(pin);
                        await SaveImagesAsync();
                        await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                    }

                }
            }
        }

        private async void OnMapTapAsync(object obj)
        {
            var position = (Position)obj;
            await AddPinViewModelOnMapAsync(position);
            Latitude = _pinViewModel.Latitude.ToString();
            Longitude = _pinViewModel.Longitude.ToString();
        }

        private void OnClearLabelTap()
        {
            Label = string.Empty;
        }

        private void OnClearDescriptionTap()
        {
            Description = string.Empty;
        }

        private void OnClearLatitudeTap()
        {
            Latitude = string.Empty;
        }

        private void OnClearLongitudeTap()
        {
            Longitude = string.Empty;
        }

        // TODO: Check without icons on IOS.
        private void OnAddImageTap()
        {
            var config = new ActionSheetConfig
            {
                Title = Resource["Alert"]
            };

            config.Add(Resource["Gallery"], new Action(async () =>
            {
                string imagePath = await _mediaService.TakePhotoFromGalleryAsync();
                CreatePinImageModel(imagePath);
            }), icon: "ic_collections_black.png");

            config.Add(Resource["Camera"], new Action(async () =>
            {
                string imagePath = await _mediaService.TakePhotoWithCameraAsync();
                CreatePinImageModel(imagePath);
            }), icon: "ic_camera_alt_black.png");

            UserDialogs.Instance.ActionSheet(config);
        }

        private async void OnDeleteImageTapAsync(PinImageModel pinImageModel)
        {
            await _pinImageService.DeletePinImageAsync(pinImageModel);
            PinImageList.Remove(pinImageModel);
            ChangeImageListHeight();
        }

        #endregion

    }
}
