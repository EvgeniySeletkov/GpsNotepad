using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Pin;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class PinsListTabPageViewModel : BaseViewModel
    {
        private IPinService _pinService;
        private ObservableCollection<PinViewModel> _pinViewModelList;

        public PinsListTabPageViewModel(INavigationService navigationService,
                                        ILocalizationService localizationService,
                                        IPinService pinService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
        }

        #region --- Public properties ---

        private ObservableCollection<PinViewModel> pinViewModelList;
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

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        private ICommand pinVisibleChangeTapCommand;
        public ICommand PinVisibleChangeTapCommand =>
            pinVisibleChangeTapCommand ?? (pinVisibleChangeTapCommand = new DelegateCommand<PinViewModel>(OnPinVisibleChangeTap));

        private ICommand selectPinCommand;
        public ICommand SelectPinCommand =>
            selectPinCommand ?? (selectPinCommand = new DelegateCommand(OnSelectPinTap));

        private ICommand addPinTapCommand;
        public ICommand AddPinTapCommand => 
            addPinTapCommand ?? (addPinTapCommand = new DelegateCommand(OnAddPinTap));

        private ICommand editPinTapCommand;
        public ICommand EditPinTapCommand =>
            editPinTapCommand ?? (editPinTapCommand = new DelegateCommand<PinViewModel>(OnEditPinTap));

        private ICommand deletePinTapCommand;
        public ICommand DeletePinTapCommand =>
            deletePinTapCommand ?? (deletePinTapCommand = new DelegateCommand<PinViewModel>(OnDeletePinTap));

        #endregion

        #region --- Private helpers ---

        private async void OnPinVisibleChangeTap(PinViewModel pinViewModel)
        {
            if (pinViewModel.IsVisible)
            {
                pinViewModel.IsVisible = false;
                pinViewModel.Image = "closed_eye.png";
            }
            else
            {
                pinViewModel.IsVisible = true;
                pinViewModel.Image = "eye.png";
            }

            var pinModel = pinViewModel.ToPinModel();

            await _pinService.UpdatePinAsync(pinModel);
        }

        private async void OnSelectPinTap()
        {
            var parameters = new NavigationParameters();
            var pin = SelectedPinViewModel.ToPin();
            parameters.Add(nameof(SelectedPinViewModel), pin);
            await NavigationService.SelectTabAsync($"{nameof(MapTabPage)}", parameters);
        }

        private async void OnAddPinTap()
        {
            await NavigationService.NavigateAsync($"{nameof(AddEditPage)}");
        }

        private async void OnEditPinTap(PinViewModel pinViewModel)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(PinViewModel), pinViewModel);
            await NavigationService.NavigateAsync($"{nameof(AddEditPage)}", parameters);
        }

        private async void OnDeletePinTap(PinViewModel pinViewModel)
        {
            var pinModel = pinViewModel.ToPinModel();
            await _pinService.DeletePinAsync(pinModel);
            PinViewModelList.Remove(pinViewModel);
        }

        #endregion

        #region --- Overrides ---

        public override async void Initialize(INavigationParameters parameters)
        {
            var pinModelList = await _pinService.GetAllPinsAsync();
            var pinViewModelList = new List<PinViewModel>();

            foreach (var pinModel in pinModelList)
            {
                var pinViewModel = pinModel.ToPinViewModel();
                //refactor
                if (pinViewModel.IsVisible)
                {
                    pinViewModel.Image = "eye.png";
                }
                else
                {
                    pinViewModel.Image = "closed_eye.png";
                }
                pinViewModelList.Add(pinViewModel);
            }

            PinViewModelList = new ObservableCollection<PinViewModel>(pinViewModelList);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            //remove
            parameters.Add(nameof(PinViewModel), PinViewModelList);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
                _pinViewModelList ??= new ObservableCollection<PinViewModel>(PinViewModelList);

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    PinViewModelList = new ObservableCollection<PinViewModel>(_pinViewModelList);
                    _pinViewModelList = null;
                }
                else
                {
                    //add method to PinService
                    PinViewModelList = new ObservableCollection<PinViewModel>(_pinViewModelList.Where(p =>
                           p.Label.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           p.Latitude.ToString().StartsWith(SearchText) || 
                           p.Longitude.ToString().StartsWith(SearchText) ||
                           (!string.IsNullOrWhiteSpace(p.Description) &&
                           p.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))));
                }
            }
        }

        #endregion

    }
}
