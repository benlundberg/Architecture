using Architecture;
using Architecture.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer_iOS))]
namespace Architecture.iOS.Renderers
{
    public class ExtendedWebViewRenderer_iOS : ViewRenderer<ExtendedWebView, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }

            if (e.OldElement != null)
            {
                // Clean up
            }

            if (e.NewElement != null)
            {
                var webview = Element as ExtendedWebView;

                string filename = webview.Uri;

                Control.LoadRequest(new NSUrlRequest(new NSUrl(filename, false)));
                Control.ScalesPageToFit = true;
            }
        }
    }
}