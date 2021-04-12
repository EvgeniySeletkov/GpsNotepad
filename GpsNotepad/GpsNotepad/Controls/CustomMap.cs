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

        //public static readonly BindableProperty CustomMapTypeProperty =
        //    BindableProperty.Create(
        //        propertyName: nameof(CustomMapType),
        //        returnType: typeof(MapType),
        //        declaringType: typeof(CustomMap),
        //        propertyChanged: CustomMapTypePropertyChanged,
        //        defaultBindingMode: BindingMode.TwoWay);

        //public MapType CustomMapType
        //{
        //    get
        //    {
        //        return (MapType)GetValue(CustomMapTypeProperty);
        //    }
        //    set
        //    {
        //        SetValue(CustomMapTypeProperty, value);
        //    }
        //}

        //private static void CustomMapTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    CustomMap customMap = (CustomMap)bindable;
        //    customMap.MapType = (MapType)newValue;
        //}

        //public static BindableProperty CameraPositionProperty =
        //    BindableProperty.Create(nameof(MapPosition),
        //        typeof(Position),
        //        typeof(CustomMap),
        //        defaultValue: new CameraPosition(),
        //        defaultBindingMode: BindingMode.TwoWay
        //        );


        //public CameraPosition MapPosition
        //{
        //    get
        //    {
        //        return (CameraPosition)GetValue(MapPositionProperty);
        //    }
        //    set
        //    {
        //        SetValue(MapPositionProperty, value);
        //    }
        //}

    }
}
