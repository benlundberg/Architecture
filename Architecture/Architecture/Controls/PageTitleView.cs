using System.Linq;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class PageTitleView : Grid
    {
        public PageTitleView()
        {
            PropertyChanged += PageTitleView_PropertyChanged;
        }

        private void PageTitleView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!IsCentered)
            {
                PropertyChanged -= PageTitleView_PropertyChanged;
                return;
            }

            if (Width > 0)
            {
                if (!(Children.First() is View view))
                {
                    return;
                }

                view.TranslationX = 0;

                if (!(Parent is ContentPage contentPage))
                {
                    return;
                }

                if (!(contentPage.Parent is NavigationPage navigationPage))
                {
                    return;
                }

                PropertyChanged -= PageTitleView_PropertyChanged;

                view.TranslationX = (this.Width - (navigationPage.Width - 16)) / 2;
            }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(string),
            declaringType: typeof(PageTitleView),
            defaultValue: default(string),
            propertyChanged: TextChanged);

        private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is PageTitleView view))
            {
                return;
            }

            view.Children.Clear();

            view.Children.Add(new Label
            {
                MaxLines = 1,
                Text = view.Text,
                TextColor = Application.Current.ToolbarTextColor(),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = view.IsCentered ? LayoutOptions.Center : LayoutOptions.Start,
                FontSize = Device.RuntimePlatform == Device.iOS ? Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)) : 20,
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(8, 0, 0, 0) : new Thickness(0)
            });
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsCentered { get; set; }
    }
}
