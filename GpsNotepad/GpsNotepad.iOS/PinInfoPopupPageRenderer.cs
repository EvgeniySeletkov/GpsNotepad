using Foundation;
using GpsNotepad.iOS;
using GpsNotepad.Views;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PinInfoPopupPage), typeof(PinInfoPopupPageRenderer))]
namespace GpsNotepad.iOS
{
    class PinInfoPopupPageRenderer : PageRenderer
    {
        private UIViewController _parentModalViewController;
        #region -- Overrides --

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            _parentModalViewController = parent;
            parent.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(false);
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(false);
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }
        #endregion
    }
}