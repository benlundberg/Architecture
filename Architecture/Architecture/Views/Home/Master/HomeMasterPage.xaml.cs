using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMasterPage : FlyoutPage
    {
        public HomeMasterPage()
        {
            InitializeComponent();

            Flyout = ViewContainer.Current.CreatePage(new MasterViewModel(this));
        }
    }
}