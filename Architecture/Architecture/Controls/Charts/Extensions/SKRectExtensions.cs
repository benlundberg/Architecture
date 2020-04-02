using SkiaSharp;
using Xamarin.Forms;

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

        public static SKPoint GetPoint(this SKRect rect, float touchedX, int itemIndex, int itemsCount, int margin, SKRect frame, StackOrientation orientation)
        {
            SKPoint point = new SKPoint();

            if (orientation == StackOrientation.Vertical)
            {
                if (touchedX - (rect.Width / 2) < frame.Left)
                {
                    point.X = frame.Left;
                }
                else if (touchedX + (rect.Width / 2) > frame.Right)
                {
                    point.X = frame.Right - rect.Width;
                }
                else
                {
                    point.X = touchedX - (rect.Width / 2);
                }

                point.Y = (rect.Height * itemIndex) + (itemIndex * margin);
            }
            else
            {
                var totalWidth = (rect.Width * itemsCount) + (itemIndex * margin);

                if (totalWidth / 2 >= touchedX - frame.Left)
                {
                    point.X = touchedX + (itemIndex * rect.Width) + (itemIndex * margin);
                }
                else if (totalWidth / 2 >= frame.Right - touchedX)
                {
                    point.X = touchedX - totalWidth + (itemIndex * rect.Width) + (itemIndex * margin) + margin;
                }
                else
                {
                    point.X = touchedX - ((totalWidth / 2) - (itemIndex * rect.Width) - (itemIndex * itemIndex));
                }

                point.Y = rect.Height;
            }

            return point;
        }
    }
}
