using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidationEntry : ContentView
    {
        public ValidationEntry()
        {
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            InitView();
        }

        private void InitView()
        {
            RootLayout.BackgroundColor = RootBackgroundColor;

            if (!string.IsNullOrEmpty(FontIcon))
            {
                EntryIcon.Text = FontIcon;
                EntryIcon.FontFamily = FontIconSolid ? (OnPlatform<string>)Application.Current.Resources["FontAwesomeSolid"] : (OnPlatform<string>)Application.Current.Resources["FontAwesomeRegular"];
                EntryIcon.IsVisible = true;
            }

            EntryField.TextColor = TextColor;
            EntryField.Placeholder = PlaceholderText;
            EntryField.PlaceholderColor = PlaceholderColor;
            EntryField.IsPassword = IsPassword;
            EntryField.MaxLength = MaxTextLength == 0 ? 255 : MaxTextLength;
            EntryField.Keyboard = Keyboard;
            EntryField.HorizontalTextAlignment = TextAlignment;
            EntryField.FontSize = Device.GetNamedSize(FontSize, typeof(Entry));

            if (!string.IsNullOrEmpty(ValidationText))
            {
                EntryField.Unfocused += (object sender, FocusEventArgs e) =>
                {
                    if (string.IsNullOrEmpty(Text))
                    {
                        EntryValidation.IsVisible = true;
                    }
                    else
                    {
                        EntryValidation.IsVisible = false;
                    }
                };

                EntryValidation.Text = ValidationText;
                EntryValidation.TextColor = ValidationTextColor;
            }

            EntryField.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                Text = e?.NewTextValue;
            };
        }

        public static readonly BindableProperty ValidationTextProperty = BindableProperty.Create(
            propertyName: "ValidationText",
            returnType: typeof(string),
            declaringType: typeof(ValidationEntry),
            defaultValue: default(string));

        public string ValidationText
        {
            get { return (string)GetValue(ValidationTextProperty); }
            set { SetValue(ValidationTextProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(string),
            declaringType: typeof(ValidationEntry),
            defaultValue: default(string));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Colors
        public Color PlaceholderColor { get; set; } = Color.Black;
        public Color TextColor { get; set; } = Color.Black;
        public Color ValidationTextColor { get; set; } = Color.IndianRed;
        public Color RootBackgroundColor { get; set; } = Color.White;

        public string FontIcon { get; set; }
        public bool FontIconSolid { get; set; }
        public string PlaceholderText { get; set; }
        public Keyboard Keyboard { get; set; }
        public NamedSize FontSize { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public int MaxTextLength { get; set; }
        public bool IsPassword { get; set; }
    }
}