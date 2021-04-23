using Acr.UserDialogs;
using GpsNotepad.Controls;
using GpsNotepad.Extensions;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MapCameraPosition;
using GpsNotepad.Services.Permission;
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
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.PlatformConfiguration;

namespace GpsNotepad.ViewModels
{
    class MapTabPageViewModel : BaseViewModel
    {
        private IPinService _pinService;
        private IMapCameraPositionService _mapCameraPositionService;
        private IPermissionService _permissionService;
        private ObservableCollection<PinViewModel> _pinViewModelList;

        public MapTabPageViewModel(INavigationService navigationService,
                                   ILocalizationService localizationService,
                                   IPinService pinService,
                                   IMapCameraPositionService mapCameraPositionService,
                                   IPermissionService permissionService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _mapCameraPositionService = mapCameraPositionService;
            _permissionService = permissionService;
        }

        #region --- Public properties ---

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

        #region --- Private helpers ---

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

        private void DisplayPinInfo(PinViewModel pinViewModel)
        {
            IsPinInfoVisible = true;
            Label = $"Label: {pinViewModel.Label}";
            Address = $"Address: {pinViewModel.Address}";
            var latitude = pinViewModel.Latitude;
            var longitude = pinViewModel.Longitude;
            Position = $"Coordinates: {latitude}; {longitude}";
            Description = $"Description: {pinViewModel.Description}";
        }

        private void OnCameraMove(object obj)
        {
            //change parameter type
            var cameraPosition = (CameraPosition)obj;
            _mapCameraPositionService.SetCameraPosition(cameraPosition);
        }

        private void OnPinViewModelSelectTap()
        {
            var pin = SelectedPinViewModel.ToPin();
            CameraPosition = new MapSpan(pin.Position, 1, 1);
            IsPinListVisible = false;
        }

        private async void OnPinSelectTap(object obj)
        {
            //change parameter type
            Pin selectedPin = (Pin)obj;
            var selectedPinViewModel = PinViewModelList.FirstOrDefault(p => p.Label == selectedPin.Label);
            var parameters = new NavigationParameters();
            parameters.Add("pinId", selectedPinViewModel.PinId);
            //DisplayPinInfo(selectedPinViewModel);
            await NavigationService.NavigateAsync($"{nameof(PinImagesPage)}", parameters);
        }

        private async void OnGoToCurrentLocationTap()
        {
            var status = await _permissionService.CheckAndRequestLocationPermission();

            if (status == PermissionStatus.Granted)
            {
                //var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync();
                var position = new Position(location.Latitude, location.Longitude);
                CameraPosition = new MapSpan(position, 1, 1);
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
            var pinViewModelList = new List<PinViewModel>();

            foreach (var pinModel in pinModelList)
            {
                var pinViewModel = pinModel.ToPinViewModel();
                pinViewModelList.Add(pinViewModel);
            }

            PinViewModelList = new ObservableCollection<PinViewModel>(pinViewModelList);

            CameraPosition = _mapCameraPositionService.GetCameraPosition();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<ObservableCollection<PinViewModel>>(nameof(PinViewModel), out var newPinViewModelList))
            {
                var pinViewModelList = new List<PinViewModel>();
                
                foreach (var pinViewModel in newPinViewModelList)
                {
                    pinViewModelList.Add(pinViewModel);
                }
                PinViewModelList = new ObservableCollection<PinViewModel>(pinViewModelList);
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
                           p.Longitude.ToString().StartsWith(SearchText) || 
                           (string.IsNullOrWhiteSpace(p.Description) && 
                           p.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))));
                    ChangeListHeight();
                }
            }
        }

        #endregion
    }
}
