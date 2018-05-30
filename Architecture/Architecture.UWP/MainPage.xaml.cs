namespace Architecture.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Bootstrapper_UWP.Initialize();

            LoadApplication(new Architecture.App());
        }
    }
}
