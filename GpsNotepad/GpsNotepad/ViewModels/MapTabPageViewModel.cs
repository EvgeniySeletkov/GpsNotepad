using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
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
    class MapTabPageViewModel : BindableBase
    {
        public MapTabPageViewModel()
        {

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

        private void OnMapTypeChangeTap()
        {
            MapType = MapType.Satellite;
        }

        private void OnMapTap(object obj)
        {
            var pin = new Pin()
            {
                Label = "Test",
                Position = (Position)obj
            };

            var pinsTest = new List<Pin>();
            pinsTest.Add(pin);
            Pins = pinsTest;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }
    }
}
