using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
		public MasterPage ()
		{
			InitializeComponent ();
		}
	}
}