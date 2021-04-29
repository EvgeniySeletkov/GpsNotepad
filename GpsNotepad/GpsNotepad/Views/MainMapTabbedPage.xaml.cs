using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GpsNotepad.Views
{
    public partial class MainMapTabbedPage : Xamarin.Forms.TabbedPage
    {
        public MainMapTabbedPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}