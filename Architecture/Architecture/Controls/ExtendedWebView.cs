using Xamarin.Forms;

namespace Architecture.Controls
{
    public class ExtendedWebView : WebView
    {
        public ExtendedWebView()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
        }

        public static readonly BindableProperty UrlProperty = BindableProperty.Create(
            propertyName: "Url",
            returnType: typeof(string),
            declaringType: typeof(ExtendedWebView),
            defaultValue: default(string));

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
    }
}
