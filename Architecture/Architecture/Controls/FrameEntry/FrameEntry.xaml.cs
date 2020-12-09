using Architecture.Core;
using FFImageLoading.Transformations;
using System;
using System.Linq;
using System.Windows.Input;
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

        protected override void OnParentSet()
        {
            base.OnParentSet();

            IsHeaderVisible = !string.IsNullOrEmpty(Text?.Value);
            InternalPlaceholder = IsHeaderVisible ? string.Empty : Placeholder;
            InternalBorderColor = BorderColor;
        }

        private void BorderlessEntry_Focused(object sender, FocusEventArgs e)
        {
            if (!IsHeaderVisible)
            {
                IsHeaderVisible = true;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await FrameHeader.FadeTo(1, length: 100);
                });
            }

            InternalBorderColor = SelectedBorderColor;
            InternalPlaceholder = string.Empty;
        }

        private void BorderlessEntry_Unfocused(object sender, FocusEventArgs e)
        {
            IsHeaderVisible = !string.IsNullOrEmpty((sender as Entry)?.Text);
            InternalPlaceholder = IsHeaderVisible ? string.Empty : Placeholder;
            InternalBorderColor = BorderColor;

            if (!IsHeaderVisible)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await FrameHeader.FadeTo(0, length: 100);
                });
            }

            if (Text != null)
            {
                if (Text.Validations?.Any() == true)
                {
                    Text.Validate();

                    IsValidationVisible = !Text.IsValid;
                }
            }
        }

        private void BorderlessEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangedCommand?.Execute(Text);

            IsValidationVisible = false;
        }

        private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FrameEntry view))
            {
                return;
            }

            if (!(newValue is ValidatableObject<string> value))
            {
                return;
            }

            view.IsHeaderVisible = !string.IsNullOrEmpty(value?.Value?.ToString());
            view.InternalPlaceholder = view.IsHeaderVisible ? string.Empty : view.Placeholder;
            view.InternalBorderColor = view.BorderColor;

            if (view.IsHeaderVisible)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await view.FrameHeader.FadeTo(1, length: 100);
                });
            }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(ValidatableObject<string>),
            declaringType: typeof(FrameEntry),
            defaultValue: default(ValidatableObject<string>),
            propertyChanged: TextChanged);

        public ValidatableObject<string> Text
        {
            get { return (ValidatableObject<string>)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextChangedCommandProperty = BindableProperty.Create(
            propertyName: "TextChangedCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(FrameEntry),
            defaultValue: default(ICommand));

        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public string InternalPlaceholder { get; set; }
        public string Placeholder { get; set; }
        public Color PlaceholderColor { get; set; } = App.Current.Get<Color>("TextColor");
        
        public bool IsHeaderVisible { get; set; }
        public Color HeaderBackgroundColor { get; set; } = Color.White;
        public Color HeaderColor { get; set; } = App.Current.Get<Color>("TextColor");
        
        public Keyboard Keyboard { get; set; }
        public Color TextColor { get; set; } = App.Current.Get<Color>("TextColor");
        public Color BorderColor { get; set; } = App.Current.Get<Color>("GrayMedium");
        public Color SelectedBorderColor { get; set; } = App.Current.Get<Color>("GrayMedium");
        public Color InternalBorderColor { get; set; } = App.Current.Get<Color>("GrayMedium");
        public bool IsPassword { get; set; }
        
        public string IconFontFamily { get; set; }
        public string IconTextSource { get; set; }
        public Color IconColor { get; set; } = App.Current.Get<Color>("TextColor");
        public bool HasIcon => !string.IsNullOrEmpty(IconTextSource);

        public string ImageSource { get; set; }
        public bool HasImage => !string.IsNullOrEmpty(ImageSource);

        public Color EntryBackground { get; set; } = Color.White;

        public bool IsValidationVisible { get; private set; }

        public int MaxLength { get; set; } = 255;

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            propertyName: "Command",
            returnType: typeof(ICommand),
            declaringType: typeof(FrameEntry),
            defaultValue: default(ICommand),
            propertyChanged: CommandPropertyChanged);

        private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FrameEntry view))
            {
                return;
            }

            view.IsCommandVisible = newValue != null;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public bool IsCommandVisible { get; set; }
        public string CommandIconFontFamily { get; set; }
        public string CommandIconTextSource { get; set; }
        public Color CommandIconColor { get; set; } = Color.White;
        public Color CommandIconBackgroundColor { get; set; } = App.Current.Get<Color>("PrimaryColor");

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Command?.Execute(null);
        }
    }
}