using GpsNotepad.Extensions;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.Pin;
using Prism.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.ViewModels
{
    class MapTabPageViewModel : BaseViewModel
    {
        private IPinService _pinService;

        public MapTabPageViewModel(INavigationService navigationService,
                                   ILocalizationService localizationService,
                                   IPinService pinService) : base(navigationService, localizationService)
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
                var pin = pinModel.GetPin();
                pinList.Add(pin);
            }

            Pins = pinList;
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
            }

            Pins = pinList;

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }
    }
}
