using GpsNotepad.Extensions;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.MapCameraPosition;
using GpsNotepad.Services.Pin;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class MapTabPageViewModel : BaseViewModel
    {
        private IPinService _pinService;
        private IMapCameraPositionService _mapCameraPositionService;

        public MapTabPageViewModel(INavigationService navigationService,
                                   ILocalizationService localizationService,
                                   IPinService pinService,
                                   IMapCameraPositionService mapCameraPositionService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _mapCameraPositionService = mapCameraPositionService;
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

        private MapSpan cameraPosition;
        public MapSpan CameraPosition
        {
            get => cameraPosition;
            set => SetProperty(ref cameraPosition, value);
        }

        private ICommand cameraMoveCommand;
        public ICommand CameraMoveCommand =>
            cameraMoveCommand ?? (cameraMoveCommand = new DelegateCommand<object>(OnCameraMove));

        private void OnCameraMove(object obj)
        {
            var cameraPosition = (CameraPosition)obj;
            _mapCameraPositionService.SetCameraPosition(cameraPosition);
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            var pinModelList = await _pinService.GetAllPinsAsync();

            var pinList = new List<Pin>();

            foreach (var pinModel in pinModelList)
            {
                var pin = pinModel.GetPin();
                pinList.Add(pin);
            }

            Pins = pinList;

            CameraPosition = _mapCameraPositionService.GetCameraPosition();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var pinList = new List<Pin>();
            int count = 0;
            foreach (var parameter in parameters)
            {
                if (parameters.TryGetValue<Pin>($"pin{count}", out var newValue))
                {
                    pinList.Add(newValue);
                    count++;
                }
                if (parameters.TryGetValue<Pin>($"SelectedPin", out newValue))
                {
                    CameraPosition = new MapSpan(newValue.Position, 1, 1);
                }
            }

            Pins = pinList;

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }
    }
}
