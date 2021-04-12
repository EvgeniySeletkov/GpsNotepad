using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Resources;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Pin;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
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

        private bool isSaveButtonEnabled = false;
        public bool IsSaveButtonEnabled
        {
            get => isSaveButtonEnabled;
            set => SetProperty(ref isSaveButtonEnabled, value);
        }

        private ICommand mapTapCommand;
        public ICommand MapTapCommand => 
            mapTapCommand ?? (mapTapCommand = new DelegateCommand<object>(OnMapTap));

        private ICommand savePinTapCommand;
        public ICommand SavePinTapCommand => 
            savePinTapCommand ?? (savePinTapCommand = new DelegateCommand(OnSavePinTap));

        private bool HasEmptyEntries()
        {
            bool isEmpty = false;
            if (string.IsNullOrWhiteSpace(LabelEntry) ||
                string.IsNullOrWhiteSpace(LatitudeEntry) ||
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

        private Pin CreatePin(Position position)
        {
            Pin pin = null;
            if (!HasEmptyEntries())
            {
                pin = new Pin()
                {
                    Label = LabelEntry,
                    Position = position,
                    IsVisible = true
                };
            }

            return pin;
        }

        private void AddPinOnMap(Position position)
        {
            var pin = CreatePin(position);
            if (pin != null)
            {
                var pinList = new List<Pin>();
                pinList.Add(pin);
                Pins = pinList;
            }

        }

        private void CreatePinModel()
        {
            _pinModel = new PinModel()
            {
                Label = LabelEntry,
                Latitude = latitude,
                Longitude = longitude,
                Description = DescriptionEntry,
                IsVisible = true
            };
        }

        private void EditPinModel()
        {
            _pinModel.Label = LabelEntry;
            _pinModel.Latitude = latitude;
            _pinModel.Longitude = longitude;
            _pinModel.Description = DescriptionEntry;
        }

        private void OnMapTap(object obj)
        {
            var position = (Position)obj;
            AddPinOnMap(position);
            LatitudeEntry = position.Latitude.ToString();
            LongitudeEntry = position.Longitude.ToString();
        }

        private async void OnSavePinTap()
        {
            if (!HasEmptyEntries())
            {
                if (HasCoordinates())
                {
                    if (_pinModel == null)
                    {
                        CreatePinModel();
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var pinModel = parameters.GetValue<PinModel>(nameof(PinModel));
            if (pinModel != null)
            {
                Title = "Edit Pin";
                _pinModel = pinModel;
                LabelEntry = pinModel.Label;
                LatitudeEntry = pinModel.Latitude.ToString();
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(LabelEntry) ||
                args.PropertyName == nameof(LatitudeEntry) ||
                args.PropertyName == nameof(LongitudeEntry))
            {
                if (!HasEmptyEntries() && HasCoordinates())
                {
                    var position = new Position(latitude, longitude);
                    AddPinOnMap(position);
                }
            }
        }
    }
}
