using Acr.UserDialogs;
using GpsNotepad.Models;
using GpsNotepad.Services.Pin;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class MapTabPageViewModel : BaseViewModel
    {
        private IPinService _pinService;

        public MapTabPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
        }

        //private CameraPosition cameraPosition;
        //public CameraPosition CameraPosition
        //{
        //    get => cameraPosition;
        //    set => SetProperty(ref cameraPosition, value);
        //}

        private List<Pin> pins = new List<Pin>();
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            var pinModelList = await _pinService.GetAllPinsAsync();

            var pinList = new List<Pin>();

            foreach (var pinModel in pinModelList)
            {
                var pin = new Pin();
                pin.Label = pinModel.Label;
                pin.Position = new Position(pinModel.Latitude, pinModel.Longitude);
                pin.Address = pinModel.Address;
                pin.IsVisible = pinModel.IsVisible;
                pinList.Add(pin);
            }

            Pins = pinList;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }
    }
}
