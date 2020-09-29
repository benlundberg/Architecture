using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    public enum SnackbarDuration
    {
        Short,
        Long
    }

    public class SnackbarOption
    {
        public SnackbarOption()
        {

        }

        public SnackbarOption(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
        public SnackbarDuration Duration { get; set; }
        public string ButtonText { get; set; }
        public ICommand Command { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SnackbarPopup : PopupPage
    {
        public SnackbarPopup(SnackbarOption option)
        {
            this.Option = option;
            
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (Option.Duration == SnackbarDuration.Short)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            await CloseAsync();
        }

        private async Task CloseAsync()
        {
            if (PopupNavigation.Instance.PopupStack.Contains(this))
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Option.Command?.Execute(null);
            await CloseAsync();
        }

        public SnackbarOption Option { get; set; }
        public Color ButtonTextColor { get; set; } = Color.Orange;
        public Color TextColor { get; set; } = Color.White;
        public bool IsButtonVisible => !string.IsNullOrEmpty(Option?.ButtonText);
    }
}