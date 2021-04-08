using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMapPage : ContentView
    {
        //private static Position _lastPosition;

        //public static readonly BindableProperty MapPositionProperty =
        //    BindableProperty.Create(nameof(Position),
        //        typeof(Position),
        //        typeof(CustomMapPage),
        //        defaultValue: 
        //        defaultBindingMode: BindingMode.TwoWay,
        //        propertyChanged: MapPositionPropertyChanged);
        
        //private static void MapPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (CustomMapPage)bindable;
        //    control.MapPosition = (Position)newValue;
        //}

        //public Position MapPosition
        //{
        //    get
        //    {
        //        return (Position)GetValue(MapPositionProperty);
        //    }
        //    set
        //    {
        //        SetValue(MapPositionProperty, value);
        //    }
        //}


        public CustomMapPage()
        {
            InitializeComponent();

            //var mapStyle =
            //    "[" +
            //    "    {" +
            //    "        \"featureType\":\"administrative\"," +
            //    "        \"elementType\":\"labels.text.fill\"," +
            //    "        \"stylers\":[" +
            //    "            {" +
            //    "                \"color\":\"#444444\"" +
            //    "            }" +
            //    "        ]" +
            //    "    }," +
            //    "    {" +
            //    "        \"featureType\":\"landscape\"," +
            //    "        \"elementType\":\"all\"," +
            //    "        \"stylers\":[" +
            //    "            {" +
            //    "                \"color\":\"#f2f2f2\"" +
            //    "            }" +
            //    "        ]" +
            //    "    }," +
            //    "    {" +
            //    "        \"featureType\":\"poi\"," +
            //    "        \"elementType\":\"all\"," +
            //    "        \"stylers\":[" +
            //    "            {" +
            //    "                \"visibility\":\"off\"" +
            //    "            }" +
            //    "        ]" +
            //    "    }," +
            //    "    {" +
            //    "        \"featureType\":\"road\"," +
            //    "        \"elementType\":\"all\"," +
            //    "        \"stylers\":[" +
            //    "            {" +
            //    "                \"saturation\":-100" +
            //    "            }," +
            //    "            {" +
            //    "                \"lightness\":45" +
            //    "            }" +
            //    "        ]" +
            //    "    }," +
            //    "{\"featureType\":\"road.highway\",\"elementType\":\"all\"," +
            //    "\"stylers\":[{\"visibility\":\"simplified\"}]}," +
            //    "{\"featureType\":\"road.arterial\"," +
            //    "\"elementType\":\"labels.icon\"," +
            //    "\"stylers\":[{\"visibility\":\"off\"}]}," +
            //    "{\"featureType\":\"transit\"," +
            //    "\"elementType\":\"all\"," +
            //    "\"stylers\":[{\"visibility\":\"off\"}]}," +
            //    "{\"featureType\":\"water\"," +
            //    "\"elementType\":\"all\"," +
            //    "\"stylers\":[{\"color\":\"#46bcec\"}," +
            //    "{\"visibility\":\"on\"}]}]";

            //map.MapStyle = MapStyle.FromJson(mapStyle);
        }
    }
}