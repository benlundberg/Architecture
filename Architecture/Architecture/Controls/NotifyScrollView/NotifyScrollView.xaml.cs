using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotifyScrollView : ContentView
    {
        public NotifyScrollView()
        {
            InitializeComponent();

            HeaderView.PropertyChanged += HeaderContent_PropertyChanged;
        }

        private void HeaderContent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (HeaderContent != null && args.PropertyName == "Height" && HeaderView.Height != -1)
            {
                headerContentHeight = HeaderView.Height;
                HeaderView.HeightRequest = 0;
                HeaderView.PropertyChanged -= HeaderContent_PropertyChanged;
            }
        }

        public View HeaderContent { get; set; }
        public View MainContent { get; set; }

        private bool isAnimating;
        private double headerContentHeight;

        private void ScrollRoot_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (isAnimating)
            {
                return;
            }

            if (e.ScrollY > ScrollMaxLimit)
            {
                if (HeaderView.HeightRequest <= 0)
                {
                    isAnimating = true;

                    HeaderView.Animate("show", (height) =>
                    {
                        HeaderView.HeightRequest = height;
                    },
                    start: 0, end: headerContentHeight, easing: Easing.CubicIn, finished: (d, b) =>
                    {
                        isAnimating = false;
                    });
                }
            }
            else
            {
                if (HeaderView.HeightRequest > 0)
                {
                    isAnimating = true;

                    HeaderView.Animate("hide", (height) =>
                    {
                        HeaderView.HeightRequest = height;
                    },
                    start: HeaderView.HeightRequest, end: 0, easing: Easing.CubicOut, finished: (d, b) =>
                    {
                        isAnimating = false;
                    });
                }
            }
        }

        // TODO: Find an automat value for this
        private int ScrollMaxLimit = 150;
    }
}