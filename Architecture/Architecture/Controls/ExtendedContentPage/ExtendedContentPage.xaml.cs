using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtendedContentPage : ContentPage
    {
        public ExtendedContentPage()
        {
            InitializeComponent();
        }

        public View View { get; set; }
    }
}