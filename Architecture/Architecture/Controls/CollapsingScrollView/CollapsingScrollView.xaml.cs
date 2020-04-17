using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollapsingScrollView : ContentView
    {
        public CollapsingScrollView()
        {
            InitializeComponent();
        }

        private double ReMap(double oldValue, double oldMin, double oldMax, double newMin, double newMax)
        {
            return (((oldValue - oldMin) / (oldMax - oldMin)) * (newMax - newMin)) + newMin;
        }

        public View HeaderContent { get; set; }
        public View MainContent { get; set; }

        private void ScrollRoot_Scrolled(object sender, ScrolledEventArgs e)
        {
            var val = ReMap(e.ScrollY, 0, ScrollMaxLimit, 1, 0);

            if (!OnlyFadeOut)
            {
                this.HeaderContent.Scale = val;
            }

            this.HeaderContent.Opacity = val;
        }

        // TODO: Find an automat value for this
        private int ScrollMaxLimit = 300;

        public bool OnlyFadeOut { get; set; }
    }
}