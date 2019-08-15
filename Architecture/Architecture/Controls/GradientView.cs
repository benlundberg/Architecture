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
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public GradientDirection Direction { get; set; }
    }
}
