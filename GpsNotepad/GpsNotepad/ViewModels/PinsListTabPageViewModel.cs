using Acr.UserDialogs;
using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Services.Pin;
using GpsNotepad.ViewModels.ExtendedViewModels;
using GpsNotepad.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public PinsListTabPageViewModel(INavigationService navigationService,
                                        IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
        }

        private ObservableCollection<PinViewModel> pins;
        public ObservableCollection<PinViewModel> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }

        public ICommand PinVisibleChangeTapCommand => new Command<PinViewModel>(OnPinVisibleChangeTap);
        public ICommand AddPinTapCommand => new Command(OnAddPinTap);

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
            await _navigationService.NavigateAsync($"{nameof(AddEditPage)}");
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

    }
}
