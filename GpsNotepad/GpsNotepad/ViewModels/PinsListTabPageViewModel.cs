using Acr.UserDialogs;
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

        public ICommand PinVisibleChangeTapCommand => new Command<PinViewModel>(OnPinVisiblechangeTap);
        public ICommand AddPinTapCommand => new Command(OnAddPinTap);

        private async void OnPinVisiblechangeTap(PinViewModel pin)
        {
            if (pin.IsVisible)
            {
                pin.IsVisible = false;
                pin.Image = "closed_eye.png";
            }
            else
            {
                pin.IsVisible = true;
                pin.Image = "eye.png";
            }

            var pinModel = new PinModel()
            {
                Id = pin.PinId,
                Label = pin.Label,
                Latitude = pin.Latitude,
                Longitude = pin.Longitude,
                Address = pin.Address,
                IsVisible = pin.IsVisible,
                UserId = pin.UserId
            };

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
                var pinViewModel = new PinViewModel();
                pinViewModel.PinId = pinModel.Id;
                pinViewModel.Label = pinModel.Label;
                pinViewModel.Latitude = pinModel.Latitude;
                pinViewModel.Longitude = pinModel.Longitude;
                pinViewModel.Address = pinViewModel.Address;
                pinViewModel.IsVisible = pinModel.IsVisible;
                pinViewModel.UserId = pinModel.UserId;
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
    }
}
