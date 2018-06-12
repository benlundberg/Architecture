using Xamarin.Forms;

namespace Architecture
{
    public class ExtendedWebView : WebView
    {
        public ExtendedWebView()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
        }

        public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
                    returnType: typeof(string),
                    declaringType: typeof(ExtendedWebView),
                    defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}
