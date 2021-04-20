using Android.Content;
using AndroidX.Core.View;
using Architecture;
using Architecture.Controls;
using Architecture.Droid;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(ExtendedCustomFrame_Droid))]
namespace Architecture.Droid
{
    public class ExtendedCustomFrame_Droid : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        public ExtendedCustomFrame_Droid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            UpdateElevation();
        }

        private void UpdateElevation()
        {
            var materialFrame = (CustomFrame)Element;

            // we need to reset the StateListAnimator to override the setting of Elevation on touch down and release.
            Control.StateListAnimator = new Android.Animation.StateListAnimator();

            // set the elevation manually
            ViewCompat.SetElevation(this, materialFrame.Elevation);
            ViewCompat.SetElevation(Control, materialFrame.Elevation);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Elevation")
            {
                UpdateElevation();
            }
        }
    }
}