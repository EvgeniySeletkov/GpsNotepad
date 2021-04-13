using System.Collections.Generic;
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
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: MapPinsPropertyChanged);

        public List<Pin> MapPins
        {
            get => (List<Pin>)GetValue(MapPinsProperty);
            set => SetValue(MapPinsProperty, value);
        }

        private static void MapPinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customMap = (CustomMap)bindable;
            customMap.Pins.Clear();
            if ((List<Pin>)newValue != null)
            {
                foreach (var pin in (List<Pin>)newValue)
                {
                    customMap.Pins.Add(pin);
                }
            }
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

        //public static readonly BindableProperty PinsSelectProperty = BindableProperty.Create(
        //    propertyName: nameof(PinsSelect),
        //    returnType: typeof(List<Pin>),
        //    declaringType: typeof(CustomMap),
        //    defaultValue: default,
        //    propertyChanged: PinsSelectPropertyChanged);

        //private static void PinsSelectPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (CustomMap)bindable;
        //    control.Pins.Clear();
        //    foreach (Pin pin in (List<Pin>)newValue)
        //    {
        //        control.Pins.Add(pin);
        //    }
        //}

        //public List<Pin> PinsSelect
        //{
        //    get => (List<Pin>)GetValue(PinsSelectProperty);
        //    set => SetValue(PinsSelectProperty, value); 
        //}

    }
}
