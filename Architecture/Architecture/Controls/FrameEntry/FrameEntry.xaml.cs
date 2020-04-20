using Architecture.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrameEntry : ContentView
    {
        public FrameEntry()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(ValidatableObject<string>),
            declaringType: typeof(FrameEntry),
            defaultValue: default(ValidatableObject<string>));

        public ValidatableObject<string> Text
        {
            get { return (ValidatableObject<string>)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            if (HasValidation)
            {
                Text?.Validate();
            }
        }

        public Keyboard Keyboard { get; set; }
        public string Placeholder { get; set; }
        public string Header { get; set; }
        public Color PlaceholderColor { get; set; } = App.Current.Get<Color>("TextColor");
        public Color HeaderBackgroundColor { get; set; } = Color.White;
        public Color HeaderColor { get; set; } = App.Current.Get<Color>("TextColor");
        public Color TextColor { get; set; } = App.Current.Get<Color>("TextColor");
        public Color BorderColor { get; set; } = App.Current.Get<Color>("TextColor");
        public bool IsPassword { get; set; }
        public string IconFontFamily { get; set; }
        public string IconTextSource { get; set; }
        public Color IconColor { get; set; } = App.Current.Get<Color>("TextColor");
        public bool HasIcon => !string.IsNullOrEmpty(IconTextSource);
        public bool HasValidation { get; set; }
    }
}