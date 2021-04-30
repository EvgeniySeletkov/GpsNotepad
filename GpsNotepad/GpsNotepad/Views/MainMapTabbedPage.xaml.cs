using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GpsNotepad.Views
{
    public partial class MainMapTabbedPage : BaseTabbedPage
    {
        public MainMapTabbedPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}