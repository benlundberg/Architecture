using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace Architecture.Droid
{
    [Activity(Label = "Architecture", Icon = "@drawable/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.SetTheme(Resource.Style.AppTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.SetFlags(
                "FastRenderers_Experimental", 
                "CollectionView_Experimental", 
                "CarouselView_Experimental", 
                "IndicatorView_Experimental",
                "SwipeView_Experimental",
                "Expander_Experimental",
                "Brush_Experimental",
                "Shapes_Experimental");

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            global::Xamarin.Forms.FormsMaterial.Init(this, bundle);

            global::Xamarin.Essentials.Platform.Init(this, bundle);

            Bootstrapper_Droid.Initialize();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

