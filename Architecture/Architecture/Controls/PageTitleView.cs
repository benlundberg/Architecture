using System.Linq;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class PageTitleView : Grid
    {
        public PageTitleView()
        {
            IsCentered = Device.RuntimePlatform == Device.iOS;
            PropertyChanged += PageTitleView_PropertyChanged;
        }

        private void PageTitleView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CenterTitle();
        }

        private void CenterTitle()
        {
            if (Width <= 0)
            {
                return;
            }

            if (IsCentered)
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

                view.HorizontalOptions = LayoutOptions.Center;
                view.TranslationX = (this.Width - (navigationPage.Width - 16)) / 2;
            }

            PropertyChanged -= PageTitleView_PropertyChanged;
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
                FontSize = 20,
                FontFamily = "OpenSansSemiBold",
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(8, 0, 0, 0) : new Thickness(0)
            });
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private bool isCentered;
        public bool IsCentered
        {
            get 
            { 
                return isCentered; 
            }
            set
            {
                isCentered = value;

                if (isCentered)
                {
                    this.CenterTitle();
                }
            }
        }
    }
}
