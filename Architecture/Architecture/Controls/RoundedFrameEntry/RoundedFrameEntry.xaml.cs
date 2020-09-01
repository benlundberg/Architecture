using Architecture.Core;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoundedFrameEntry : ContentView
    {
        public RoundedFrameEntry()
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

            TextChangedCommand?.Execute(Text);
        }

        private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RoundedFrameEntry view))
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
            declaringType: typeof(RoundedFrameEntry),
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
            declaringType: typeof(RoundedFrameEntry),
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
        public Color BorderColor { get; set; } = App.Current.Get<Color>("TextColor");
        public Color SelectedBorderColor { get; set; } = App.Current.Get<Color>("TextColor");
        public Color InternalBorderColor { get; set; } = App.Current.Get<Color>("TextColor");
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
    }
}