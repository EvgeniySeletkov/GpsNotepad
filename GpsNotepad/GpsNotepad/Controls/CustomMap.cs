using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Controls
{
    class CustomMap : Map
    {
        public static readonly BindableProperty MapPinsProperty =
            BindableProperty.Create(
                propertyName: nameof(MapPins),
                returnType: typeof(List<Pin>),
                declaringType: typeof(CustomMap),
                defaultValue: default(List<Pin>),
                defaultBindingMode: BindingMode.TwoWay);

        public List<Pin> MapPins
        {
            get => (List<Pin>)GetValue(MapPinsProperty);
            set => SetValue(MapPinsProperty, value);
        }

        public static readonly BindableProperty MoveToPositionProperty = BindableProperty.Create(
            propertyName: nameof(MoveToPosition),
            returnType: typeof(MapSpan),
            declaringType: typeof(CustomMap),
            defaultValue: default,
            propertyChanged: MoveToPositionPropertyChanged);

        public MapSpan MoveToPosition
        {
            get => (MapSpan)GetValue(MoveToPositionProperty);
            set => SetValue(MoveToPositionProperty, value);
        }

        private static void MoveToPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMap)bindable;
            control.MoveToRegion((MapSpan)newValue);

        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(MapPins)) 
            {
                Pins.Clear();

                if (MapPins != null )
                {
                    foreach (var pin in MapPins)
                    {
                        Pins.Add(pin);
                    }
                }
            }
        }
    }
}
