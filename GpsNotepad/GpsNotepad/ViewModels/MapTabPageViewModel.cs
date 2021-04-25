using GpsNotepad.Extensions;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MapCameraPosition;
using GpsNotepad.Services.Permission;
using GpsNotepad.Services.Pin;
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


namespace GpsNotepad.ViewModels
{
    class MapTabPageViewModel : BaseViewModel
    {
        private readonly IPinService _pinService;
        private readonly IMapCameraPositionService _mapCameraPositionService;
        private readonly IPermissionService _permissionService;
        private List<PinViewModel> _pinViewModelListForSearch;

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

        private ObservableCollection<PinViewModel> _pinList = new ObservableCollection<PinViewModel>();
        public ObservableCollection<PinViewModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
        }

        private PinViewModel _selectedPin;
        public PinViewModel SelectedPin
        {
            get => _selectedPin;
            set => SetProperty(ref _selectedPin, value);
        }

        private bool _isMyLocation;
        public bool IsMyLocation
        {
            get => _isMyLocation;
            set => SetProperty(ref _isMyLocation, value);
        }

        private bool _isPinListVisible;
        public bool IsPinListVisible
        {
            get => _isPinListVisible;
            set => SetProperty(ref _isPinListVisible, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private int _listHeight;
        public int ListHeight
        {
            get => _listHeight;
            set => SetProperty(ref _listHeight, value);
        }

        private MapSpan cameraPosition;
        public MapSpan CameraPosition
        {
            get => cameraPosition;
            set => SetProperty(ref cameraPosition, value);
        }

        private ICommand _cameraMoveCommand;
        public ICommand CameraMoveCommand =>
            _cameraMoveCommand ??= new DelegateCommand<CameraPosition>(OnCameraMove);

        private ICommand _foundedPinSelectCommand;
        public ICommand FoundedPinSelectCommand =>
            _foundedPinSelectCommand ??= new DelegateCommand(OnFoundedPinSelectTap);

        private ICommand _pinSelectCommand;
        public ICommand PinSelectCommand =>
            _pinSelectCommand ??= new DelegateCommand<Pin>(OnPinSelectTapAsync);

        #endregion

        #region --- Overrides ---

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var status = await _permissionService.RequestLocationPermissionAsync();

            IsMyLocation = status == PermissionStatus.Granted ? true : false;

            var pinModelList = await _pinService.GetAllPinsAsync();
            var pinViewModelList = new List<PinViewModel>();

            foreach (var pinModel in pinModelList)
            {
                var pinViewModel = pinModel.ToPinViewModel();
                pinViewModelList.Add(pinViewModel);
            }

            PinList = new ObservableCollection<PinViewModel>(pinViewModelList);

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
                PinList = new ObservableCollection<PinViewModel>(pinViewModelList);
            }
            if (parameters.TryGetValue<Pin>(nameof(SelectedPin), out var newPin))
            {
                CameraPosition = new MapSpan(newPin.Position, 1, 1);
            }

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SearchText))
            {
                _pinViewModelListForSearch ??= new List<PinViewModel>(PinList);

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    IsPinListVisible = false;
                    PinList = new ObservableCollection<PinViewModel>(_pinViewModelListForSearch);
                    _pinViewModelListForSearch = null;
                }
                else
                {
                    IsPinListVisible = true;
                    PinList = new ObservableCollection<PinViewModel>(_pinViewModelListForSearch.Where(p =>
                           p.Label.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           p.Latitude.ToString().StartsWith(SearchText) ||
                           p.Longitude.ToString().StartsWith(SearchText) ||
                           (!string.IsNullOrWhiteSpace(p.Description) &&
                           p.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))));
                    ChangeListHeight();
                }
            }
        }

        #endregion

        #region --- Private helpers ---

        private void ChangeListHeight()
        {
            if (PinList.Count < 3)
            {
                ListHeight = PinList.Count * 63;
            }
            else
            {
                ListHeight = 126;
            }
        }

        private void OnCameraMove(CameraPosition cameraPosition)
        {
            _mapCameraPositionService.SaveCameraPosition(cameraPosition);
        }

        private void OnFoundedPinSelectTap()
        {
            var pin = SelectedPin.ToPin();
            CameraPosition = new MapSpan(pin.Position, 1, 1);
            IsPinListVisible = false;
        }

        private async void OnPinSelectTapAsync(Pin selectedPin)
        {
            var selectedPinViewModel = PinList.FirstOrDefault(p => p.Label == selectedPin.Label);
            var parameters = new NavigationParameters();
            parameters.Add(nameof(PinViewModel), selectedPinViewModel);
            await NavigationService.NavigateAsync(nameof(PinInfoPopupPage), parameters, true, true);
        }

        #endregion

    }
}
