using SkiaSharp;

namespace Architecture.Controls.Charts
{
    public static class SKRectExtensions
    {
        public static float GetInsideXValue(this SKRect rect, float x)
        {
            if (x < rect.Left)
            {
                return rect.Left;
            }
            else if (x > rect.Right)
            {
                return rect.Right;
            }

            return x;
        }

        public static float GetInsideYValue(this SKRect rect, float y, int padding = 0)
        {
            if (y == 0)
            {
                return rect.MidY;
            }

            if (y <= rect.Top + padding)
            {
                return rect.Top + padding;
            }
            else if (y >= rect.Bottom - padding)
            {
                return rect.Bottom - padding;
            }

            return y;
        }

        public static float GetItemWidth(this SKRect rect, int maxItems)
        {
            return rect.Width / (maxItems - 1);
        }
    }
}
