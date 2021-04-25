using GpsNotepad.Extensions;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Localization;
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

namespace GpsNotepad.ViewModels
{
    class PinsListTabPageViewModel : BaseViewModel
    {
        private readonly IPinService _pinService;
        private ObservableCollection<PinViewModel> _pinViewModelList;

        public PinsListTabPageViewModel(INavigationService navigationService,
                                        ILocalizationService localizationService,
                                        IPinService pinService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
        }

        #region --- Public properties ---

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

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private ICommand _pinVisibleChangeTapCommand;
        public ICommand PinVisibleChangeTapCommand =>
            _pinVisibleChangeTapCommand ??= new DelegateCommand<PinViewModel>(OnPinVisibleChangeTapAsync);

        private ICommand _selectPinCommand;
        public ICommand SelectPinCommand =>
            _selectPinCommand ??= new DelegateCommand(OnSelectPinTapAsync);

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

                pinViewModel.Image = pinViewModel.IsVisible ? "ic_like_blue.png" : "ic_like_gray.png";
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
                _pinViewModelList ??= new ObservableCollection<PinViewModel>(PinList);

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    PinList = new ObservableCollection<PinViewModel>(_pinViewModelList);
                    _pinViewModelList = null;
                }
                else
                {
                    //add method to PinService
                    PinList = new ObservableCollection<PinViewModel>(_pinViewModelList.Where(p =>
                           p.Label.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           p.Latitude.ToString().StartsWith(SearchText) ||
                           p.Longitude.ToString().StartsWith(SearchText) ||
                           (!string.IsNullOrWhiteSpace(p.Description) &&
                           p.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))));
                }
            }
        }

        #endregion

        #region --- Private helpers ---

        private async void OnPinVisibleChangeTapAsync(PinViewModel pinViewModel)
        {
            if (pinViewModel.IsVisible)
            {
                pinViewModel.IsVisible = false;
                pinViewModel.Image = "ic_like_gray.png";
            }
            else
            {
                pinViewModel.IsVisible = true;
                pinViewModel.Image = "ic_like_blue.png";
            }

            var pinModel = pinViewModel.ToPinModel();

            await _pinService.UpdatePinAsync(pinModel);
        }

        private async void OnSelectPinTapAsync()
        {
            var parameters = new NavigationParameters();
            var pin = SelectedPin.ToPin();
            parameters.Add(nameof(SelectedPin), pin);
            await NavigationService.SelectTabAsync($"{nameof(MapTabPage)}", parameters);
        }

        private async void OnAddPinTapAsync()
        {
            await NavigationService.NavigateAsync($"{nameof(AddEditPage)}");
        }

        private async void OnEditPinTapAsync(PinViewModel pinViewModel)
        {
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
