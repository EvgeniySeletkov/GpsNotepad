using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Pin;
using GpsNotepad.ViewModels.ExtendedViewModels;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
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
        private ObservableCollection<PinViewModel> _pinList;

        public PinsListTabPageViewModel(INavigationService navigationService,
                                        ILocalizationService localizationService,
                                        IPinService pinService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
        }

        private ObservableCollection<PinViewModel> pins;
        public ObservableCollection<PinViewModel> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
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

        private ICommand pinVisibleChangeTapCommand;
        public ICommand PinVisibleChangeTapCommand =>
            pinVisibleChangeTapCommand ?? (pinVisibleChangeTapCommand = new DelegateCommand<PinViewModel>(OnPinVisibleChangeTap));

        private ICommand selectionChangedCommand;
        public ICommand SelectionChangedCommand =>
            selectionChangedCommand ?? (selectionChangedCommand = new DelegateCommand(OnSelectionChanged));

        private ICommand addPinTapCommand;
        public ICommand AddPinTapCommand => 
            addPinTapCommand ?? (addPinTapCommand = new DelegateCommand(OnAddPinTap));

        private ICommand editPinTapCommand;
        public ICommand EditPinTapCommand =>
            editPinTapCommand ?? (editPinTapCommand = new DelegateCommand<PinViewModel>(OnEditPinTap));

        private ICommand deletePinTapCommand;
        public ICommand DeletePinTapCommand =>
            deletePinTapCommand ?? (deletePinTapCommand = new DelegateCommand<PinViewModel>(OnDeletePinTap));

        private ICommand searchCommand;
        public ICommand SearchCommand =>
            searchCommand ?? (searchCommand = new DelegateCommand(OnSearch));

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

            var pinModel = pinViewModel.GetPinModel();

            await _pinService.SavePinAsync(pinModel);
        }

        private async void OnAddPinTap()
        {
            await NavigationService.NavigateAsync($"{nameof(AddEditPage)}");
        }

        private async void OnEditPinTap(PinViewModel pinViewModel)
        {
            var pinModel = pinViewModel.GetPinModel();
            var parameters = new NavigationParameters();
            parameters.Add(nameof(PinModel), pinModel);
            await NavigationService.NavigateAsync($"{nameof(AddEditPage)}", parameters);
        }

        private async void OnDeletePinTap(PinViewModel pinViewModel)
        {
            var pinModel = pinViewModel.GetPinModel();
            await _pinService.DeletePinAsync(pinModel);
            Pins.Remove(pinViewModel);
        }

        private void OnSearch()
        {
        //    if (string.IsNullOrWhiteSpace(SearchText))
        //    {
        //        Pins = new ObservableCollection<PinViewModel>(_pinList);
        //    }
        //    else
        //    {
        //        Pins = new ObservableCollection<PinViewModel>(_pinList.Where(p => p.Label.Contains(SearchText)));
        //    }
        }

        private async void OnSelectionChanged()
        {
            var parameters = new NavigationParameters();
            var pin = SelectedPin.GetPin();
            parameters.Add(nameof(SelectedPin), pin);
            await NavigationService.SelectTabAsync($"{nameof(MapTabPage)}", parameters);
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            var pinModelList = await _pinService.GetAllPinsAsync();

            var pinList = new List<PinViewModel>();

            foreach (var pinModel in pinModelList)
            {
                var pinViewModel = pinModel.GetPinViewModel();
                if (pinViewModel.IsVisible)
                {
                    pinViewModel.Image = "eye.png";
                }
                else
                {
                    pinViewModel.Image = "closed_eye.png";
                }
                pinList.Add(pinViewModel);
            }

            Pins = new ObservableCollection<PinViewModel>(pinList);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            int count = 0;
            foreach (var pinViewModel in Pins)
            {
                var pin = pinViewModel.GetPin();
                parameters.Add($"{nameof(pin)}{count}", pin);
                count++;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
                _pinList ??= new ObservableCollection<PinViewModel>(Pins);

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    Pins = new ObservableCollection<PinViewModel>(_pinList);
                    _pinList = null;
                }
                else
                {
                    Pins = new ObservableCollection<PinViewModel>(_pinList.Where(p =>
                           p.Label.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           p.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           p.Latitude.ToString().StartsWith(SearchText) || 
                           p.Longitude.ToString().StartsWith(SearchText)));
                }
            }
        }

    }
}
