using Architecture.Core;
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

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(
            propertyName: "Value",
            returnType: typeof(ValidatableObject<string>),
            declaringType: typeof(ValidationEntry),
            defaultValue: default(ValidatableObject<string>));

        public ValidatableObject<string> Value
        {
            get { return (ValidatableObject<string>)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            Value.Validate();
        }

        public Keyboard Keyboard { get; set; }
        public string Placeholder { get; set; }
        public Color PlaceholderColor { get; set; }
        public Color TextColor { get; set; }
        public bool IsPassword { get; set; }
    }
}