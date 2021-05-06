using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GpsNotepad.Views
{
    public partial class MainMapTabbedPage : BaseTabbedPage
    {
        public MainMapTabbedPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
        }
    }
}