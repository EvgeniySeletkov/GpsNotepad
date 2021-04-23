using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Resources;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Pin;
using GpsNotepad.ViewModels.ExtendedViewModels;
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
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class AddEditPageViewModel : BaseViewModel
    {
        private PinViewModel _pinViewModel;

        private IPinService _pinService;

        public AddEditPageViewModel(INavigationService navigationService,
                                    ILocalizationService localizationService,
                                    IPinService pinService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
        }

        #region --- Public properties ---

        private string title = "Add Pin";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        //check for using anywhere
        private List<Pin> pins = new List<Pin>();
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }

        //initialize in ctor
        private ObservableCollection<PinViewModel> pinViewModelList = new ObservableCollection<PinViewModel>();
        public ObservableCollection<PinViewModel> PinViewModelList
        {
            get => pinViewModelList;
            set => SetProperty(ref pinViewModelList, value);
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

        private string latitideEntry = "0";
        public string LatitudeEntry
        {
            get => latitideEntry;
            set => SetProperty(ref latitideEntry, value);
        }

        private string longitideEntry = "0";
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
            mapTapCommand ?? (mapTapCommand = new DelegateCommand<object>(OnMapTap));

        private ICommand savePinTapCommand;
        public ICommand SavePinTapCommand => 
            savePinTapCommand ?? (savePinTapCommand = new DelegateCommand(OnSavePinTap));

        #endregion
        //regions order
        #region --- Private helpers ---

        private bool HasEmptyCordinates()
        {
            bool isEmpty = false;
            if (string.IsNullOrWhiteSpace(LatitudeEntry) ||
                string.IsNullOrWhiteSpace(LongitudeEntry))
            {
                isEmpty = true;
                Pins.Clear();
            }

            return isEmpty;
        }

        private async Task<string> GetAddressAsync(Position position)
        {
            //move this method to service
            var geocoder = new Geocoder();
            var addressList = await geocoder.GetAddressesForPositionAsync(position);
            var fullAddress = addressList != null ? addressList.FirstOrDefault() : string.Empty;
            var address = !string.IsNullOrWhiteSpace(fullAddress) ? 
                (fullAddress.Substring(0, 
                             fullAddress.IndexOf(",") != -1 ?
                             fullAddress.IndexOf(",") :
                             fullAddress.Length)) : string.Empty;

            return address;
        }

        private async Task<PinViewModel> CreatePinViewModelAsync(Position position)
        {
            var address = await GetAddressAsync(position);

            _pinViewModel = new PinViewModel()
            {
                //string to resources
                Label = !string.IsNullOrWhiteSpace(LabelEntry) ? LabelEntry : "Untitled pin",
                Latitude = Math.Round(position.Latitude, 2),
                Longitude = Math.Round(position.Longitude, 2),
                Address = address,
                IsVisible = true,
                Description = DescriptionEntry
            };

            return _pinViewModel;
        }

        //EditPinAsync
        private async Task<PinViewModel> EditPinViewModelAsync(Position position)
        {
            var address = await GetAddressAsync(position);
            _pinViewModel.Label = !string.IsNullOrWhiteSpace(LabelEntry) ? LabelEntry : "Untitled pin";
            _pinViewModel.Latitude = Math.Round(position.Latitude, 2);
            _pinViewModel.Longitude = Math.Round(position.Longitude, 2);
            _pinViewModel.Address = address;
            _pinViewModel.Description = DescriptionEntry;

            return _pinViewModel;
        }

        private async Task<PinViewModel> AddPinViewModelOnMapAsync(Position position)
        {
            //refactor (return types)
            if (_pinViewModel == null)
            {
                await CreatePinViewModelAsync(position);
                
            }
            else
            {
                await EditPinViewModelAsync(position);
            }

            var pinViewModelList = new List<PinViewModel>();
            pinViewModelList.Add(_pinViewModel);
            PinViewModelList = new ObservableCollection<PinViewModel>(pinViewModelList);

            return _pinViewModel;
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

        private async void OnSavePinTap()
        {
            if (!string.IsNullOrWhiteSpace(LabelEntry) && !HasEmptyCordinates())
            {
                var pinModels = await _pinService.GetAllPinsAsync();
                var uniquePinModel = pinModels.FirstOrDefault(p => p.Label == LabelEntry);
                if (uniquePinModel == null)
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

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //use TryGetValue
            var pinViewModel = parameters.GetValue<PinViewModel>(nameof(PinViewModel));
            if (pinViewModel != null)
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
        }

        #endregion

    }
}
