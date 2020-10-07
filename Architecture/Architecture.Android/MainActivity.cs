using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Architecture.Core;

namespace Architecture.Droid
{
    [Activity(
        Label = "Architecture", 
        Icon = "@drawable/ic_launcher", 
        Theme = "@style/SplashTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTop)]
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

            CreateNotificationFromIntent(Intent);
        }

        private void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras == null)
            {
                return;
            }

            string title = intent.Extras.GetString(NotificationService_Droid.TitleKey);
            string message = intent.Extras.GetString(NotificationService_Droid.MessageKey);

            ComponentContainer.Current.Resolve<INotificationService>().ReceiveNotification(title, message);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

