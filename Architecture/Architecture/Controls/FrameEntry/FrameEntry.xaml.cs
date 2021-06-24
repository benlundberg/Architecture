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

            IsHeaderVisible = HeaderShouldBeDisplayed && !string.IsNullOrEmpty(Text?.Value);
            InternalPlaceholder = IsHeaderVisible ? string.Empty : Placeholder;
            InternalBorderColor = BorderColor;
        }

        private void BorderlessEntry_Focused(object sender, FocusEventArgs e)
        {
            if (!IsHeaderVisible && HeaderShouldBeDisplayed)
            {
                IsHeaderVisible = true;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await FrameHeader.FadeTo(1, length: 100);
                });
            }

            InternalBorderColor = SelectedBorderColor;
            InternalPlaceholder = IsHeaderVisible ? string.Empty : Placeholder;
        }

        private void BorderlessEntry_Unfocused(object sender, FocusEventArgs e)
        {
            IsHeaderVisible = HeaderShouldBeDisplayed && !string.IsNullOrEmpty((sender as Entry)?.Text);
            InternalPlaceholder = IsHeaderVisible ? string.Empty : Placeholder;
            InternalBorderColor = BorderColor;

            if (!IsHeaderVisible)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await FrameHeader.FadeTo(0, length: 100);
                });
            }

            if (Text != null && UnfocusValidation)
            {
                if (Text.Validations?.Any() == true)
                {
                    Text.Validate();
                }
            }
        }

        private void BorderlessEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangedCommand?.Execute(Text);

            if (Text != null)
            {
                Text.IsValid = true;
            }
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

            view.IsHeaderVisible = view.HeaderShouldBeDisplayed && !string.IsNullOrEmpty(value?.Value?.ToString());
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

        // Placeholder
        public string InternalPlaceholder { get; set; }
        public string Placeholder { get; set; }
        public Color PlaceholderColor { get; set; } = App.Current.Get<Color>("GrayDark");
        
        // Floating header label
        public bool IsHeaderVisible { get; set; }
        public Color HeaderBackgroundColor { get; set; } = App.Current.Get<Color>("PageBackgroundColor");
        public Color HeaderTextColor { get; set; } = App.Current.Get<Color>("TextColor");
        public bool HeaderShouldBeDisplayed { get; set; } = true;

        // Border
        public Color InternalBorderColor { get; set; } = App.Current.Get<Color>("GrayMedium");
        public double BorderCornerRadius => HasRoundedCorners ? 28 : 4;
        public Color BorderColor { get; set; } = App.Current.Get<Color>("GrayDark");
        public Color SelectedBorderColor { get; set; } = App.Current.PrimaryColor();
        public bool HasRoundedCorners { get; set; } 

        // Icon
        public string IconFontFamily { get; set; }
        public string Icon { get; set; }
        public Color IconColor { get; set; } = App.Current.Get<Color>("TextColor");
        public bool HasIcon => !string.IsNullOrEmpty(Icon);

        // Image
        public string ImageSource { get; set; }
        public bool HasImage => !string.IsNullOrEmpty(ImageSource);

        // Validation
        public bool UnfocusValidation { get; set; } = true;

        // Common entry properties
        public Color EntryBackgroundColor { get; set; } = App.Current.Get<Color>("PageBackgroundColor");
        public Color TextColor { get; set; } = App.Current.Get<Color>("TextColor");
        public int MaxLength { get; set; } = 255;
        public bool IsPassword { get; set; }
        public Keyboard Keyboard { get; set; }
        public ClearButtonVisibility ClearButtonVisibility { get; set; }

        #region Command

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
        public string CommandIcon { get; set; }
        public Color CommandIconColor { get; set; } = Color.White;
        public Color CommandIconBackgroundColor { get; set; } = App.Current.PrimaryColor();

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Command?.Execute(null);
        }

        #endregion
    }
}