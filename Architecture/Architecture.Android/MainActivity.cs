using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Architecture.Droid
{
    [Activity(Label = "Architecture", Icon = "@drawable/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            UserDialogs.Init(this);
            Bootstrapper_Droid.Initialize();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

