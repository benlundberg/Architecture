using Xamarin.Forms;

namespace Architecture.Controls
{
    public enum GradientDirection
    {
        ToRight,
        ToLeft,
        ToTop,
        ToBottom,
        ToTopLeft,
        ToTopRight,
        ToBottomLeft,
        ToBottomRight
    }

    public class GradientView : StackLayout
    {
        public GradientView()
        {
            IsClippedToBounds = true;
        }

        public Color StartColor { get; set; } = Application.Current.PrimaryColor();
        public Color EndColor { get; set; } = Application.Current.DarkPrimaryColor();
        public GradientDirection Direction { get; set; }
    }
}
