using SkiaSharp;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public static class SKPointExtensions
    {
        public static SKPoint GetAlignment(this SKPoint point, string text, SKPaint paint, TextAlignment textAlignment)
        {
            var bounds = paint.GetBounds(text);

            float x, y;

            x = point.X - bounds.Width;

            if (textAlignment == TextAlignment.Center)
            {
                y = point.Y + (paint.TextSize / 4);
            }
            else if (textAlignment == TextAlignment.End)
            {
                y = point.Y + (paint.TextSize / 2);
            }
            else
            {
                y = point.Y;
            }

            return new SKPoint(x, y);
        }
    }
}
