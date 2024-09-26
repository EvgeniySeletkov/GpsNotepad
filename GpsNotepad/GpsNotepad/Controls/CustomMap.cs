using GpsNotepad.Extensions;
using GpsNotepad.Models.Pin;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Controls
{
    class CustomMap : Map
    {
        public CustomMap()
        {
        }

        public static readonly BindableProperty MapPinViewModelsProperty =
            BindableProperty.Create(
                propertyName: nameof(MapPinViewModels),
                returnType: typeof(ObservableCollection<PinViewModel>),
                declaringType: typeof(CustomMap),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ObservableCollection<PinViewModel> MapPinViewModels
        {
            get => (ObservableCollection<PinViewModel>)GetValue(MapPinViewModelsProperty);
            set => SetValue(MapPinViewModelsProperty, value);
        }

        public static readonly BindableProperty MoveToPositionProperty = BindableProperty.Create(
            propertyName: nameof(MoveToPosition),
            returnType: typeof(MapSpan),
            declaringType: typeof(CustomMap),
            defaultValue: default);

        public MapSpan MoveToPosition
        {
            get => (MapSpan)GetValue(MoveToPositionProperty);
            set => SetValue(MoveToPositionProperty, value);
        }

        public static readonly BindableProperty IsMyLocationButtonVisibleProperty = BindableProperty.Create(
            propertyName: nameof(IsMyLocationButtonVisible),
            returnType: typeof(bool),
            declaringType: typeof(CustomMap),
            defaultValue: default);

        public bool IsMyLocationButtonVisible
        {
            get => (bool)GetValue(IsMyLocationButtonVisibleProperty);
            set => SetValue(IsMyLocationButtonVisibleProperty, value);
        }

        public static readonly BindableProperty IsZoomButtonsEnabledProperty =
            BindableProperty.Create(
                propertyName: nameof(IsZoomButtonsEnabled),
                returnType: typeof(bool),
                declaringType: typeof(CustomMap),
                defaultValue: true,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsZoomButtonsEnabled
        {
            get => (bool)GetValue(IsZoomButtonsEnabledProperty);
            set => SetValue(IsZoomButtonsEnabledProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(MapPinViewModels):
                    Pins.Clear();

                    if (MapPinViewModels != null)
                    {
                        foreach (var pinViewModel in MapPinViewModels)
                        {
                            var pin = pinViewModel.ToPin();
                            Pins.Add(pin);
                        }
                    }
                    break;
                case nameof(MoveToPosition):
                    MoveToRegion(MoveToPosition);
                    break;
                case nameof(IsMyLocationButtonVisible):
                    UiSettings.CompassEnabled = IsMyLocationButtonVisible;
                    MyLocationEnabled = IsMyLocationButtonVisible;
                    UiSettings.MyLocationButtonEnabled = IsMyLocationButtonVisible;
                    break;
                case nameof(IsZoomButtonsEnabled):
                    UiSettings.ZoomControlsEnabled = IsZoomButtonsEnabled;
                    break;
            }
        }
    }
}
