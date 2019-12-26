using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms.Platform.UWP;

namespace Architecture.UWP
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Bootstrapper_UWP.Initialize();

            Xamarin.Forms.Forms.SetFlags(
                "FastRenderers_Experimental",
                "CollectionView_Experimental",
                "CarouselView_Experimental",
                "IndicatorView_Experimental");

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            Xamarin.Forms.Forms.Init(e);

            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }

            try
            {
                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
                {
                    var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

                    if (titleBar != null)
                    {
                        titleBar.ButtonBackgroundColor = Xamarin.Forms.Application.Current.DarkPrimaryColor().ToWindowsColor();
                        titleBar.BackgroundColor = Xamarin.Forms.Application.Current.DarkPrimaryColor().ToWindowsColor();
                        titleBar.InactiveBackgroundColor = Xamarin.Forms.Application.Current.AccentColor().ToWindowsColor();
                        titleBar.ButtonInactiveBackgroundColor = Xamarin.Forms.Application.Current.AccentColor().ToWindowsColor();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
