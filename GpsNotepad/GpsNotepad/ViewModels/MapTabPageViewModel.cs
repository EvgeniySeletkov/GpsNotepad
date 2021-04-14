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

        #region --- Public properties ---

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

        private PinViewModel selectedPinViewModel;
        public PinViewModel SelectedPinViewModel
        {
            get => selectedPinViewModel;
            set => SetProperty(ref selectedPinViewModel, value);
        }

        private bool isPinListVisible = false;
        public bool IsPinListVisible
        {
            get => isPinListVisible;
            set => SetProperty(ref isPinListVisible, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
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

        private ICommand pinViewModelSelectCommand;
        public ICommand PinViewModelSelectCommand =>
            pinViewModelSelectCommand ?? (pinViewModelSelectCommand = new DelegateCommand(OnPinViewModelSelectTap));

        private ICommand pinSelectCommand;
        public ICommand PinSelectCommand => 
            pinSelectCommand ?? (pinSelectCommand = new DelegateCommand<object>(OnPinSelectTap));

        private ICommand goToCurrentLocationCommand;
        public ICommand GoToCurrentLocationCommand =>
            goToCurrentLocationCommand ?? (goToCurrentLocationCommand = new DelegateCommand(OnGoToCurrentLocationTap));

        private ICommand mapTapCommand;
        public ICommand MapTapCommand =>
            mapTapCommand ?? (mapTapCommand = new DelegateCommand<object>(OnMapTap));

        #endregion

        #region --- Private methods ---

        private void ChangeListHeight()
        {
            if (PinViewModelList.Count < 4)
            {
                ListHeight = PinViewModelList.Count * 72;
            }
            else
            {
                ListHeight = 216;
            }
        }

        private void DisplayPinInfo(int id)
        {
            IsPinInfoVisible = true;
            var pinViewModel = PinViewModelList[id];
            Label = $"Label: {pinViewModel.Label}";
            Address = $"Address: {pinViewModel.Address}";
            var latitude = pinViewModel.Latitude;
            var longitude = pinViewModel.Longitude;
            Position = $"Coordinates: {latitude}; {longitude}";
            Description = $"Description: {pinViewModel.Description}";
        }

        #endregion

        #region --- Private helpers ---

        private void OnCameraMove(object obj)
        {
            var cameraPosition = (CameraPosition)obj;
            _mapCameraPositionService.SetCameraPosition(cameraPosition);
        }

        private void OnPinViewModelSelectTap()
        {
            var pin = SelectedPinViewModel.GetPin();
            CameraPosition = new MapSpan(pin.Position, 1, 1);
            IsPinListVisible = false;
        }

        private void OnPinSelectTap(object obj)
        {
            Pin selectedPin = (Pin)obj;
            int selectedPinId = Pins.IndexOf(selectedPin);
            DisplayPinInfo(selectedPinId);
        }

        private async void OnGoToCurrentLocationTap()
        {
            try
            {
                //var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync();
                var position = new Position(location.Latitude, location.Longitude);
                CameraPosition = new MapSpan(position, 1, 1);
            }
            catch
            {
                await UserDialogs.Instance.AlertAsync("Geolocation is off!", Resource["Alert"], "OK");
            }
        }

        private void OnMapTap(object obj)
        {
            IsPinInfoVisible = false;
        }

        #endregion

        #region --- Overrides ---

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
            if (parameters.TryGetValue<Pin>(nameof(SelectedPinViewModel), out var newPin))
            {
                CameraPosition = new MapSpan(newPin.Position, 1, 1);
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

        #endregion
    }
}
