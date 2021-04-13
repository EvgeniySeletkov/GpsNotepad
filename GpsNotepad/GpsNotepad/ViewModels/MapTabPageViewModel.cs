using Acr.UserDialogs;
using GpsNotepad.Controls;
using GpsNotepad.Extensions;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MapCameraPosition;
using GpsNotepad.Services.Pin;
using GpsNotepad.ViewModels.ExtendedViewModels;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class MapTabPageViewModel : BaseViewModel
    {
        private IPinService _pinService;
        private IMapCameraPositionService _mapCameraPositionService;
        private ObservableCollection<PinViewModel> _pinViewModelList;

        public MapTabPageViewModel(INavigationService navigationService,
                                   ILocalizationService localizationService,
                                   IPinService pinService,
                                   IMapCameraPositionService mapCameraPositionService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _mapCameraPositionService = mapCameraPositionService;
        }

        private List<Pin> pins = new List<Pin>();
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }

        private ObservableCollection<PinViewModel> pinViewModelList = new ObservableCollection<PinViewModel>();
        public ObservableCollection<PinViewModel> PinViewModelList
        {
            get => pinViewModelList;
            set => SetProperty(ref pinViewModelList, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        private PinViewModel selectedPin;
        public PinViewModel SelectedPin
        {
            get => selectedPin;
            set => SetProperty(ref selectedPin, value);
        }

        private string label;
        public string Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }

        private string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        private string position;
        public string Position
        {
            get => position;
            set => SetProperty(ref position, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private bool isPinListVisible = false;
        public bool IsPinListVisible
        {
            get => isPinListVisible;
            set => SetProperty(ref isPinListVisible, value);
        }

        private int listHeight;
        public int ListHeight
        {
            get => listHeight;
            set => SetProperty(ref listHeight, value);
        }

        private bool isPinInfoVisible = false;
        public bool IsPinInfoVisible
        {
            get => isPinInfoVisible;
            set => SetProperty(ref isPinInfoVisible, value);
        }

        private MapSpan cameraPosition;
        public MapSpan CameraPosition
        {
            get => cameraPosition;
            set => SetProperty(ref cameraPosition, value);
        }

        private ICommand cameraMoveCommand;
        public ICommand CameraMoveCommand =>
            cameraMoveCommand ?? (cameraMoveCommand = new DelegateCommand<object>(OnCameraMove));

        private ICommand selectionChangedCommand;
        public ICommand SelectionChangedCommand =>
            selectionChangedCommand ?? (selectionChangedCommand = new DelegateCommand(OnSelectionChanged));

        private ICommand pinSelectCommand;
        public ICommand PinSelectCommand => 
            pinSelectCommand ?? (pinSelectCommand = new DelegateCommand<object>(OnPinSelectTap));

        private ICommand mapTapCommand;
        public ICommand MapTapCommand =>
            mapTapCommand ?? (mapTapCommand = new DelegateCommand<object>(OnMapTap));

        private ICommand goToCurrentLocationCommand;
        public ICommand GoToCurrentLocationCommand =>
            goToCurrentLocationCommand ?? (goToCurrentLocationCommand = new DelegateCommand(OnGoToCurrentLocation));

        private void ChangeListHeight()
        {
            if (PinViewModelList.Count < 4)
            {
                ListHeight = PinViewModelList.Count * 70;
            }
            else
            {
                ListHeight = 210;
            }
        }

        private void DisplayPinInfo(int id)
        {
            IsPinInfoVisible = true;
            var pinViewModel = PinViewModelList[id];
            Label = $"Label: {pinViewModel.Label}";
            Address = $"Address: {pinViewModel.Address}";
            var latitude = Math.Round(pinViewModel.Latitude, 2);
            var longitude = Math.Round(pinViewModel.Longitude, 2);
            Position = $"Coordinates: {latitude}; {longitude}";
            Description = $"Description: {pinViewModel.Description}";
        }

        private void OnCameraMove(object obj)
        {
            var cameraPosition = (CameraPosition)obj;
            _mapCameraPositionService.SetCameraPosition(cameraPosition);
        }

        private void OnSelectionChanged()
        {
            var pin = SelectedPin.GetPin();
            CameraPosition = new MapSpan(pin.Position, 1, 1);
            IsPinListVisible = false;
        }

        private void OnPinSelectTap(object obj)
        {
            Pin selectedPin = (Pin)obj;
            int selectedPinId = Pins.IndexOf(selectedPin);
            DisplayPinInfo(selectedPinId);
        }

        private void OnMapTap(object obj)
        {
            IsPinInfoVisible = false;
        }

        private async void OnGoToCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            try
            {
                var location = await Geolocation.GetLocationAsync(request);
                var position = new Position(location.Latitude, location.Longitude);
                CameraPosition = new MapSpan(position, 1, 1);
            }
            catch
            {
                await UserDialogs.Instance.AlertAsync("Geolocation is off!", Resource["Alert"], "OK");
            }
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            
            var pinModelList = await _pinService.GetAllPinsAsync();

            var pinViewModels = new List<PinViewModel>();
            var pinList = new List<Pin>();

            foreach (var pinModel in pinModelList)
            {
                var pin = pinModel.GetPin();
                var pinViewModel = pinModel.GetPinViewModel();
                pinList.Add(pin);
                pinViewModels.Add(pinViewModel);
            }

            Pins = pinList;
            PinViewModelList = new ObservableCollection<PinViewModel>(pinViewModels);

            CameraPosition = _mapCameraPositionService.GetCameraPosition();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<ObservableCollection<PinViewModel>>(nameof(PinViewModel), out var newPinViewModelList))
            {
                var pinList = new List<Pin>();
                PinViewModelList = newPinViewModelList;
                foreach (var pinViewModel in newPinViewModelList)
                {
                    pinList.Add(pinViewModel.GetPin());
                }
                Pins = pinList;
            }
            if (parameters.TryGetValue<Pin>($"SelectedPin", out var newValue))
            {
                CameraPosition = new MapSpan(newValue.Position, 1, 1);
            }

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            
            if (args.PropertyName == nameof(SearchText))
            {
                _pinViewModelList ??= new ObservableCollection<PinViewModel>(PinViewModelList);

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    IsPinListVisible = false;
                    PinViewModelList = new ObservableCollection<PinViewModel>(_pinViewModelList);
                    _pinViewModelList = null;
                }
                else
                {
                    IsPinListVisible = true;
                    PinViewModelList = new ObservableCollection<PinViewModel>(_pinViewModelList.Where(p =>
                           p.Label.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           p.Latitude.ToString().StartsWith(SearchText) ||
                           p.Longitude.ToString().StartsWith(SearchText)));
                    ChangeListHeight();
                }
            }
        }
    }
}
