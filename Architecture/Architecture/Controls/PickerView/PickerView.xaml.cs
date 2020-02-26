using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickerView : ContentView
    {
        public PickerView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: "ItemsSource",
            returnType: typeof(IList<string>),
            declaringType: typeof(PickerView),
            defaultValue: default(IList<string>));

        public IList<string> ItemsSource
        {
            get { return (IList<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
           propertyName: "SelectedItem",
           returnType: typeof(string),
           declaringType: typeof(PickerView),
           defaultValue: default(string));

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private ICommand selectCommand;
        public ICommand SelectCommand => selectCommand ?? (selectCommand = new Command(async () =>
        {
            var item = await Application.Current.MainPage.DisplayActionSheet(Title, CancelText, null, ItemsSource?.ToArray());

            if (item != null && item != CancelText)
            {
                SelectedItem = item;
            }
        }));

        public string Title { get; set; }
        public string PickerText => !string.IsNullOrEmpty(SelectedItem) ? SelectedItem : Placeholder;
        public Color TitleColor { get; set; } = Color.Black;
        public string CancelText { get; set; }
        public string Placeholder { get; set; }
        public Color PlaceholderColor { get; set; } = App.Current.AccentColor();
        public Color TextColor { get; set; } = App.Current.AccentColor();
        public string IconFontFamily { get; set; }
        public string IconTextSource { get; set; }
        public Color IconColor { get; set; } = Color.Black;
        public bool HasIcon => !string.IsNullOrEmpty(IconTextSource);
    }
}