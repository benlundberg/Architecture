using Architecture.Core;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    public class InputOption
    {
        public Keyboard Keyboard { get; set; }
        public string Placeholder { get; set; }
        public Color PlaceholderColor { get; set; } = Color.Gray;
        public Color TextColor { get; set; } = Color.Black;
        public bool IsPassword { get; set; }
        public int MaxLength { get; set; } = 255;
        public string PrimaryCommandText { get; set; } = "Ok";
        public LayoutOptions VerticalPosition { get; set; } = LayoutOptions.CenterAndExpand;
        public IPopupAnimation Animation { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputPopup : PopupPage
    {
        public InputPopup(ValidatableObject<string> text, string title, InputOption inputOption = null)
        {
            InitializeComponent();

            if (inputOption == null)
            {
                this.Option = new InputOption
                {
                    Keyboard = Keyboard.Default
                };
            }
            else
            {
                this.Option = inputOption;
            }

            if (Option.Animation == null)
            {
                this.Animation = new FadeAnimation() 
                { 
                    DurationIn = 100, 
                    DurationOut = 100
                };
            }
            else
            {
                this.Animation = Option.Animation;
            }

            this.Title = title;
            this.Text = text;
        }

        public async Task<string> ShowAsync()
        {
            taskCompletionSource = new TaskCompletionSource<string>();

            await PopupNavigation.Instance.PushAsync(this);

            await taskCompletionSource.Task;

            if (PopupNavigation.Instance.PopupStack.Contains(this))
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }

            return taskCompletionSource.Task.Result;
        }

        private ICommand primaryCommand;
        public ICommand PrimaryCommand => primaryCommand ?? (primaryCommand = new Command(() =>
        {
            if (Text.Validations?.Any() == true)
            {
                if (!Text.Validate())
                {
                    App.Current.MainPage.DisplayAlert("", Text.Error, "Ok");
                    return;
                }
            }

            taskCompletionSource.SetResult(Text.Value);
        }));

        private ICommand secondaryCommand;
        public ICommand SecondaryCommand => secondaryCommand ?? (secondaryCommand = new Command(() =>
        {
            taskCompletionSource.SetResult(null);
        }));

        public ValidatableObject<string> Text { get; set; }
        public InputOption Option { get; set; }

        private TaskCompletionSource<string> taskCompletionSource;
    }
}