using CoreGraphics;
using GpsNotepad.IOS;
using GpsNotepad.Views;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BaseTabbedPage), typeof(CustomTabbedPageRenderer))]
namespace GpsNotepad.IOS
{
    class CustomTabbedPageRenderer : TabbedRenderer
    {
        private bool _isEnable;

        public UIImage ImageWithColor(CGSize size)
        {
            CGRect rect = new CGRect(0, 0, size.Width, size.Height);
            UIGraphics.BeginImageContext(size);

            using (CGContext context = UIGraphics.GetCurrentContext())
            {
                var baseTabbedPage = (BaseTabbedPage)Element;
                var selectedTabFillColor = baseTabbedPage.SelectedTabFillColor.ToCGColor();
  
                context.SetFillColor(selectedTabFillColor);
                context.FillRect(rect);
            }

            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            nfloat bottom = 0; ;

            if (!_isEnable)
            {
                bottom = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom;
                _isEnable = !_isEnable;
            }
            
            CGSize selectedTabSize = new CGSize(TabBar.Frame.Width / TabBar.Items.Length, TabBar.Frame.Height + bottom);
            CGSize tabbarBackgroundSize = new CGSize(TabBar.Frame.Width, TabBar.Frame.Height + bottom);


            //Background Color
            //UITabBar.Appearance.BackgroundColor = UIColor.Red;
            UITabBar.Appearance.SelectionIndicatorImage = ImageWithColor(selectedTabSize);
            


            ////Normal title Color
            //UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
            ////Selected title Color
            //UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.Black }, UIControlState.Selected);
        }
    }
}