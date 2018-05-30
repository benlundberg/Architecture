using Architecture.Droid;
using Architecture.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntry_Droid))]
namespace Architecture.Droid
{
    public class BorderlessEntry_Droid : EntryRenderer
    {
        public BorderlessEntry_Droid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}