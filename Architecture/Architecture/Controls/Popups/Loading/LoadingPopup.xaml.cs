using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup : PopupPage
    {
        public LoadingPopup(string text)
        {
            InitializeComponent();

            this.Text = text;
        }

        public async Task HideAsync()
        {
            if (PopupNavigation.Instance.PopupStack.Contains(this))
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }

        public string Text { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public Color ActivityColor { get; set; } = Color.White;
    }
}