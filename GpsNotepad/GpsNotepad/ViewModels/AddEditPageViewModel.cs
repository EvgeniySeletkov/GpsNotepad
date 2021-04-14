using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Resources;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Pin;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
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
        private PinModel _pinModel;
        private double latitude;
        private double longitude;

        private IPinService _pinService;

        public AddEditPageViewModel(INavigationService navigationService,
                                    ILocalizationService localizationService,
                                    IPinService pinService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
        }

        #region --- Public properties ---

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private List<Pin> pins = new List<Pin>();
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
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

        //private bool isSaveButtonEnabled = false;
        //public bool IsSaveButtonEnabled
        //{
        //    get => isSaveButtonEnabled;
        //    set => SetProperty(ref isSaveButtonEnabled, value);
        //}

        private ICommand mapTapCommand;
        public ICommand MapTapCommand => 
            mapTapCommand ?? (mapTapCommand = new DelegateCommand<object>(OnMapTap));

        private ICommand savePinTapCommand;
        public ICommand SavePinTapCommand => 
            savePinTapCommand ?? (savePinTapCommand = new DelegateCommand(OnSavePinTap));

        #endregion

        #region --- Private methods ---

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

        private bool HasCoordinates()
        {
            bool areCoordinates = false;
            bool isLatitude = double.TryParse(LatitudeEntry, out latitude);
            bool isLongitude = double.TryParse(LongitudeEntry, out longitude);
            if (isLatitude && isLongitude)
            {
                areCoordinates = true;
            }
            return areCoordinates;
        }

        private async Task<string> GetAddressAsync(Position position)
        {
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

        private async Task<Pin> CreatePinAsync(Position position)
        {
            var address = await GetAddressAsync(position);

            Pin pin = new Pin()
            {
                Label = "Untitled pin",
                Position = position,
                Address = address,
                IsVisible = true
            };

            return pin;
        }

        private async Task<Pin> AddPinOnMapAsync(Position position)
        {
            var pin = await CreatePinAsync(position);
            if (pin != null)
            {
                var pinList = new List<Pin>();
                pinList.Add(pin);
                Pins = pinList;
            }
            return pin;
        }

        private async Task CreatePinModelAsync()
        {
            var position = new Position(latitude, longitude);
            var address = await GetAddressAsync(position);
            _pinModel = new PinModel()
            {
                Label = LabelEntry,
                Latitude = Math.Round(latitude, 2),
                Longitude = Math.Round(longitude, 2),
                Address = address,
                Description = DescriptionEntry,
                IsVisible = true
            };
        }

        private void EditPinModel()
        {
            _pinModel.Label = LabelEntry;
            _pinModel.Address = AddressEntry;
            _pinModel.Latitude = Math.Round(latitude, 2);
            _pinModel.Longitude = Math.Round(longitude, 2);
            _pinModel.Description = DescriptionEntry;
        }

        #endregion

        #region --- Private helpers ---

        private async void OnMapTap(object obj)
        {
            var position = (Position)obj;
            var pin = await AddPinOnMapAsync(position);
            AddressEntry = pin.Address;
            LatitudeEntry = pin.Position.Latitude.ToString();
            LongitudeEntry = pin.Position.Longitude.ToString();
        }

        private async void OnSavePinTap()
        {
            if (!string.IsNullOrWhiteSpace(LabelEntry) && !HasEmptyCordinates())
            {
                if (HasCoordinates())
                {
                    if (_pinModel == null)
                    {
                        await CreatePinModelAsync();
                    }
                    else
                    {
                        EditPinModel();
                    }
                    await _pinService.SavePinAsync(_pinModel);
                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                }
                else
                {
                    UserDialogs.Instance.Alert("Latitude or Longitude field is not digit!", Resource["Alert"], "OK");
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Name, Latitude or Longitude field is empty!", Resource["Alert"], "OK");
            }
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var pinModel = parameters.GetValue<PinModel>(nameof(PinModel));
            if (pinModel != null)
            {
                Title = "Edit Pin";
                _pinModel = pinModel;
                LabelEntry = pinModel.Label;
                AddressEntry = pinModel.Address
;                LatitudeEntry = pinModel.Latitude.ToString();
                LongitudeEntry = pinModel.Longitude.ToString();
                DescriptionEntry = pinModel.Description;
                var pin = pinModel.GetPin();
                Pins.Add(pin);
                var postion = new Position(pinModel.Latitude, pinModel.Longitude);
                CameraPosition = new MapSpan(postion, 1, 1);
            }
            else
            {
                Title = "Add Pin";
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(LatitudeEntry) ||
                args.PropertyName == nameof(LongitudeEntry))
            {
                if (!HasEmptyCordinates() && HasCoordinates())
                {
                    var position = new Position(latitude, longitude);
                    await AddPinOnMapAsync(position);
                }
            }
        }

        #endregion

    }
}
