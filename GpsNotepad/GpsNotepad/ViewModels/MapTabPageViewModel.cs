using GpsNotepad.Extensions;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MapCameraPosition;
using GpsNotepad.Services.Permission;
using GpsNotepad.Services.Pin;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;


namespace GpsNotepad.ViewModels
{
    class MapTabPageViewModel : BaseViewModel
    {
        private List<PinViewModel> _pinSearchList;

        private readonly IPinService _pinService;
        private readonly IMapCameraPositionService _mapCameraPositionService;
        private readonly IPermissionService _permissionService;
        private readonly IAuthorizationService _authorizationService;

        public MapTabPageViewModel(INavigationService navigationService,
                                   ILocalizationService localizationService,
                                   IPinService pinService,
                                   IMapCameraPositionService mapCameraPositionService,
                                   IPermissionService permissionService,
                                   IAuthorizationService authorizationService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _mapCameraPositionService = mapCameraPositionService;
            _permissionService = permissionService;
            _authorizationService = authorizationService;

            _pinList = new ObservableCollection<PinViewModel>();
        }

        #region --- Public properties ---

        private bool _isSearchBarFocused;
        public bool IsSearchBarFocused
        {
            get => _isSearchBarFocused;
            set => SetProperty(ref _isSearchBarFocused, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private ObservableCollection<PinViewModel> _pinList;
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

        private int _listHeight;
        public int ListHeight
        {
            get => _listHeight;
            set => SetProperty(ref _listHeight, value);
        }

        private MapSpan _cameraPosition;
        public MapSpan CameraPosition
        {
            get => _cameraPosition;
            set => SetProperty(ref _cameraPosition, value);
        }

        private ICommand _openSettingsTapCommand;
        public ICommand OpenSettingsTapCommand =>
            _openSettingsTapCommand ??= new DelegateCommand(OnOpenSettingsTapAsync);

        private ICommand _unfocusedSearchBarTapCommand;
        public ICommand UnfocusedSearchBarTapCommand =>
            _unfocusedSearchBarTapCommand ??= new DelegateCommand(OnUnfocusedSearchBarTap);

        private ICommand _logOutTapCommand;
        public ICommand LogOutTapCommand =>
            _logOutTapCommand ??= new DelegateCommand(OnLogOutTapAsync);

        private ICommand _cameraMoveCommand;
        public ICommand CameraMoveCommand =>
            _cameraMoveCommand ??= new DelegateCommand<CameraPosition>(OnCameraMove);

        private ICommand _foundedPinSelectCommand;
        public ICommand FoundedPinSelectCommand =>
            _foundedPinSelectCommand ??= new DelegateCommand<PinViewModel>(OnFoundedPinSelectTap);

        private ICommand _pinSelectCommand;
        public ICommand PinSelectCommand =>
            _pinSelectCommand ??= new DelegateCommand<Pin>(OnPinSelectTapAsync);

        #endregion

        #region --- Overrides ---

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var status = await _permissionService.RequestLocationPermissionAsync();

            IsMyLocation = status == PermissionStatus.Granted;

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
            if (parameters.TryGetValue<Pin>(nameof(Pin), out var newPin))
            {
                CameraPosition = new MapSpan(newPin.Position, 1, 1);
            }

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(IsSearchBarFocused):
                    IsPinListVisible = IsSearchBarFocused;
                    ChangeListHeight();
                    break;
                case nameof(SearchText):
                    _pinSearchList ??= new List<PinViewModel>(PinList);

                    if (string.IsNullOrWhiteSpace(SearchText))
                    {

                        PinList = new ObservableCollection<PinViewModel>(_pinSearchList);
                        _pinSearchList = null;

                    }
                    else
                    {
                        PinList = new ObservableCollection<PinViewModel>(_pinService.SearchPin(_pinSearchList, SearchText));
                    }

                    ChangeListHeight();
                    break;
            }
        }

        #endregion

        #region --- Private helpers ---

        private void ChangeListHeight()
        {
           if (PinList.Count < 3)
            {
                ListHeight = PinList.Count * 72;
            }
            else
            {
                ListHeight = 144;
            }
        }

        private async void OnOpenSettingsTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(SettingsPage));
        }

        private void OnUnfocusedSearchBarTap()
        {
            IsSearchBarFocused = false;
        }

        private async void OnLogOutTapAsync()
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(WelcomePage)}");
        }

        private void OnCameraMove(CameraPosition cameraPosition)
        {
            _mapCameraPositionService.SaveCameraPosition(cameraPosition);
        }

        private void OnFoundedPinSelectTap(PinViewModel selectedPin)
        {
            var pin = selectedPin;
            CameraPosition = new MapSpan(new Position(pin.Latitude, pin.Longitude), 1, 1);
            IsSearchBarFocused = false;
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
