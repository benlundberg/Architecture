using Architecture.Core;
using SkiaSharp;

namespace Architecture.Controls.Charts
{
    public static class SKPaintExtensions
    {
        public static SKRect GetBounds(this SKPaint paint, string text, float x, float y, SKRect padding)
        {
            try
            {
                var bounds = new SKRect();
                paint.MeasureText(text, ref bounds);

                var paddedBounds = new SKRect(bounds.Left - padding.Left, bounds.Top - padding.Top, bounds.Right + padding.Right, bounds.Bottom + padding.Bottom);

                return new SKRect(x - paddedBounds.Width, y - (paddedBounds.Height * 2), x + paddedBounds.Width, y + paddedBounds.Height);
            }
            catch (System.Exception ex)
            {
                ex.Print();
            }

            return new SKRect(x - padding.Width, y - (padding.Height * 2), x + padding.Width, y + padding.Height);
        }

        public static SKRect GetBounds(this SKPaint paint, string text, SKRect padding = new SKRect())
        {
            var bounds = new SKRect();
            paint.MeasureText(text, ref bounds);

            return new SKRect(bounds.Left - padding.Left, bounds.Top - padding.Top, bounds.Right + padding.Right, bounds.Bottom + padding.Bottom);
        }
    }
}
