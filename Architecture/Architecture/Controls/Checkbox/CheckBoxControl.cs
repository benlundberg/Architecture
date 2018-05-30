using Xamarin.Forms;

namespace Architecture.Controls
{
    public class CheckBoxControl : StackLayout
    {
        public CheckBoxControl()
        {
            this.Orientation = StackOrientation.Horizontal;
            Margin = new Thickness(8);
        }

        private void SetView()
        {
            Grid imageGrid = new Grid() { HeightRequest = 40, WidthRequest = 40 };

            Image gridBgImage = new Image()
            {
                Source = "checkOff.png",
            };

            Image image = new Image()
            {
                Source = IsChecked ? "checkOn.png" : "checkOff.png",
            };

            imageGrid.Children.Add(gridBgImage);
            imageGrid.Children.Add(image);

            Label label = new Label()
            {
                Text = Title,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Black,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            TapGestureRecognizer tap = new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    IsChecked = !IsChecked;
                    image.Source = IsChecked ? "checkOn.png" : "checkOff.png";
                })
            };

            this.GestureRecognizers.Add(tap);

            this.Children.Add(imageGrid);
            this.Children.Add(label);
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(propertyName: "IsChecked",
            returnType: typeof(bool),
            declaringType: typeof(CheckBoxControl),
            defaultValue: default(bool));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; SetView(); }
        }
    }
}
