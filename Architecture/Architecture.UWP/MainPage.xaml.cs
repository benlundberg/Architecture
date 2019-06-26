namespace Architecture.UWP
{
    public sealed partial class MainPage : Xamarin.Forms.Platform.UWP.WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.LoadApplication(new Architecture.App());
        }
    }
}
