using SkiaSharp;

namespace Architecture.Controls.Charts
{
    public static class SKPaintExtensions
    {
        public static SKRect GetBounds(this SKPaint paint, string text, float x, float y, SKRect padding)
        {
            var bounds = new SKRect();
            paint.MeasureText(text, ref bounds);

            var paddedBounds = new SKRect(bounds.Left - padding.Left, bounds.Top - padding.Top, bounds.Right + padding.Right, bounds.Bottom + padding.Bottom);

            return new SKRect(x - paddedBounds.Width, y - (paddedBounds.Height * 2), x + paddedBounds.Width, y + paddedBounds.Height);
        }

        public static SKRect GetBounds(this SKPaint paint, string text, SKRect padding = new SKRect())
        {
            var bounds = new SKRect();
            paint.MeasureText(text, ref bounds);

            return new SKRect(bounds.Left - padding.Left, bounds.Top - padding.Top, bounds.Right + padding.Right, bounds.Bottom + padding.Bottom);
        }
    }
}
