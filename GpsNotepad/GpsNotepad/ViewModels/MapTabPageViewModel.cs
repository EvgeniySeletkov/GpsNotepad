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

        private MapType mapType;
        public MapType MapType
        {
            get => mapType;
            set => SetProperty(ref mapType, value);
        }

        public ICommand MapTapCommand => new Command<object>(OnMapTap);
        public ICommand MapTypeChangeCommand => new Command(OnMapTypeChangeTap);

        private Pin CreatePin(Position pos)
        {
            Pin pin = new Pin()
            {
                Label = "Test",
                Position = pos
            };

            return pin;
        }

        private void OnMapTypeChangeTap()
        {
            MapType = MapType.Satellite;
        }

        private async void OnMapTap(object obj)
        {
            var pin = CreatePin((Position)obj);
            var pinsTest = new List<Pin>();
            pinsTest.Add(pin);
            Pins = pinsTest;

            var pinModel = new PinModel()
            {
                Label = pin.Label,
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                Address = pin.Address
            };

            await _pinService.SavePinAsync(pinModel);
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
