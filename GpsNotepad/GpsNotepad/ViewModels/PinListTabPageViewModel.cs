using GpsNotepad.Extensions;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Authorization;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Pin;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class PinListTabPageViewModel : BaseViewModel
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;
        private List<PinViewModel> _pinSearchList;

        public PinListTabPageViewModel(INavigationService navigationService,
                                        ILocalizationService localizationService,
                                        IPinService pinService,
                                        IAuthorizationService authorizationService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;
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

        private ICommand _openSettingsTapCommand;
        public ICommand OpenSettingsTapCommand =>
            _openSettingsTapCommand ??= new DelegateCommand(OnOpenSettingsTapAsync);

        private ICommand _unfocusedSearchBarTapCommand;
        public ICommand UnfocusedSearchBarTapCommand =>
            _unfocusedSearchBarTapCommand ??= new DelegateCommand(OnUnfocusedSearchBarTap);

        private ICommand _logOutTapCommand;
        public ICommand LogOutTapCommand =>
            _logOutTapCommand ??= new DelegateCommand(OnLogOutTapAsync);

        private ICommand _pinVisibleChangeTapCommand;
        public ICommand PinVisibleChangeTapCommand =>
            _pinVisibleChangeTapCommand ??= new DelegateCommand<PinViewModel>(OnPinVisibleChangeTapAsync);

        private ICommand _selectPinCommand;
        public ICommand SelectPinCommand =>
            _selectPinCommand ??= new DelegateCommand<PinViewModel>(OnSelectPinTapAsync);

        private ICommand _addPinTapCommand;
        public ICommand AddPinTapCommand => 
            _addPinTapCommand ??= new DelegateCommand(OnAddPinTapAsync);

        private ICommand _editPinTapCommand;
        public ICommand EditPinTapCommand =>
            _editPinTapCommand ??= new DelegateCommand<PinViewModel>(OnEditPinTapAsync);

        private ICommand _deletePinTapCommand;
        public ICommand DeletePinTapCommand =>
            _deletePinTapCommand ??= new DelegateCommand<PinViewModel>(OnDeletePinTapAsync);

        #endregion

        #region --- Overrides ---

        public override async void Initialize(INavigationParameters parameters)
        {
            var pinModelList = await _pinService.GetAllPinsAsync();
            var pinViewModelList = new List<PinViewModel>();

            foreach (var pinModel in pinModelList)
            {
                var pinViewModel = pinModel.ToPinViewModel();

                pinViewModel.Image = pinViewModel.IsFavorite ? "ic_like_blue.png" : "ic_like_gray.png";
                pinViewModelList.Add(pinViewModel);

            }

            PinList = new ObservableCollection<PinViewModel>(pinViewModelList);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(nameof(PinViewModel), PinList);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
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
            }
        }

        #endregion

        #region --- Private helpers ---

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

        private async void OnPinVisibleChangeTapAsync(PinViewModel pinViewModel)
        {
            if (pinViewModel.IsFavorite)
            {
                pinViewModel.IsFavorite = false;
                pinViewModel.Image = "ic_like_gray.png";
            }
            else
            {
                pinViewModel.IsFavorite = true;
                pinViewModel.Image = "ic_like_blue.png";
            }

            var pinModel = pinViewModel.ToPinModel();

            await _pinService.UpdatePinAsync(pinModel);
        }

        private async void OnSelectPinTapAsync(PinViewModel pinViewModel)
        {
            IsSearchBarFocused = false;
            var parameters = new NavigationParameters();
            var pin = pinViewModel.ToPin();
            parameters.Add(nameof(Pin), pin);
            await NavigationService.SelectTabAsync($"{nameof(MapTabPage)}", parameters);
        }

        private async void OnAddPinTapAsync()
        {
            IsSearchBarFocused = false;
            await NavigationService.NavigateAsync($"{nameof(AddEditPage)}");
        }

        private async void OnEditPinTapAsync(PinViewModel pinViewModel)
        {
            IsSearchBarFocused = false;
            var parameters = new NavigationParameters();
            parameters.Add(nameof(PinViewModel), pinViewModel);
            await NavigationService.NavigateAsync($"{nameof(AddEditPage)}", parameters);
        }

        private async void OnDeletePinTapAsync(PinViewModel pinViewModel)
        {
            var pinModel = pinViewModel.ToPinModel();
            await _pinService.DeletePinAsync(pinModel);
            PinList.Remove(pinViewModel);
        }

        #endregion

    }
}
