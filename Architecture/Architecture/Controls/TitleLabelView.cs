using Xamarin.Forms;

namespace Architecture.Controls
{
    public class TitleLabelView : Label
    {
        public TitleLabelView()
        {
            MaxLines = 1;
            FontFamily = Application.Current.NormalFont();
            TextColor = Color.White;
            VerticalOptions = LayoutOptions.CenterAndExpand;

            if (Device.RuntimePlatform == Device.iOS)
            {
                Margin = new Thickness(8, 0, 0, 0);
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label));
            }
            else
            {
                FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label));
            }
        }
    }
}
