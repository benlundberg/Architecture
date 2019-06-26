using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FloatingEntry : ContentView
    {
        public FloatingEntry()
        {
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            EntryField.TextColor = TextColor;
            EntryField.PlaceholderColor = PlaceholderColor;
            EntryField.IsPassword = IsPassword;
            EntryField.MaxLength = MaxTextLength == 0 ? 255 : MaxTextLength;
            EntryField.Keyboard = Keyboard;

            LabelTitle.TextColor = PlaceholderColor;
            LabelTitle.TranslationX = 10;
            LabelTitle.FontSize = placeholderFontSize;
        }

        private static async void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as FloatingEntry;
            if (!control.EntryField.IsFocused)
            {
                if (!string.IsNullOrEmpty((string)newValue))
                {
                    await control.TransitionToTitle(false);
                }
                else
                {
                    await control.TransitionToPlaceholder(false);
                }
            }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text",
            typeof(string),
            typeof(string),
            string.Empty,
            BindingMode.TwoWay,
            null,
            HandleBindingPropertyChangedDelegate);

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            "Title",
            typeof(string),
            typeof(string),
            string.Empty,
            BindingMode.TwoWay,
            null);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        private async void EntryField_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                await TransitionToPlaceholder(true);
            }
        }

        private async void EntryField_Focused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                await TransitionToTitle(true);
            }
        }

        private async Task TransitionToTitle(bool animated)
        {
            if (animated)
            {
                var t1 = LabelTitle.TranslateTo(0, topMargin, 100);
                var t2 = SizeTo(titleFontSize);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                LabelTitle.TranslationX = 0;
                LabelTitle.TranslationY = -30;
                LabelTitle.FontSize = 14;
            }
        }

        private async Task TransitionToPlaceholder(bool animated)
        {
            if (animated)
            {
                var t1 = LabelTitle.TranslateTo(10, 0, 100);
                var t2 = SizeTo(placeholderFontSize);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                LabelTitle.TranslationX = 10;
                LabelTitle.TranslationY = 0;
                LabelTitle.FontSize = placeholderFontSize;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            EntryField.Focus();
        }

        private Task SizeTo(int fontSize)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            // setup information for animation
            void callback(double input) { LabelTitle.FontSize = input; }
            double startingHeight = LabelTitle.FontSize;
            double endingHeight = fontSize;
            uint rate = 5;
            uint length = 100;
            Easing easing = Easing.Linear;

            // now start animation with all the setup information
            LabelTitle.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing, (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }

        public new void Focus()
        {
            EntryField.Focus();
        }

        public Color PlaceholderColor { get; set; } = Color.Black;
        public Color TextColor { get; set; } = Color.Black;
        public Keyboard Keyboard { get; set; }
        public int MaxTextLength { get; set; }
        public bool IsPassword { get; set; }

        private int placeholderFontSize = 18;
        private int titleFontSize = 14;
        private int topMargin = -32;
    }
}