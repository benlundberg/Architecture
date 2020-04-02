using Architecture.Core;
using SkiaSharp;
using System;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Only really use this if you want different from y value start.
        /// </summary>
        public static float GetVerticalAlignment(this float yPosition, float textSize, TextAlignment textAlignment = TextAlignment.Start)
        {
            float y;
            
            using (var paint = new SKPaint
            {
                TextSize = textSize
            })
            {
                if (textAlignment == TextAlignment.Center)
                {
                    y = yPosition + (textSize / 4);
                }
                else if (textAlignment == TextAlignment.End)
                {
                    y = yPosition + (textSize / 2);
                }
                else
                {
                    y = yPosition;
                }
            }

            return y;
        }

        public static float GetLowestValue(this float value, float compareValue)
        {
            return value < compareValue ? value : compareValue;
        }

        public static float GetHighestValue(this float value, float compareValue)
        {
            return value > compareValue ? value : compareValue;
        }

        public static float GetXAlignment(this float value, SKTextAlign alignment, SKRect bounds)
        {
            if (alignment == SKTextAlign.Left)
            {
                value -= bounds.Width;
            }
            else if (alignment == SKTextAlign.Right)
            {
                value += bounds.Width / 2;
            }

            return value;
        }

        public static Tuple<float, float> GetYPointAndValue(this float touchedXPoint, ChartValueItem first, ChartValueItem next)
        {
            if (next == null)
            {
                return new Tuple<float, float>(first.Point.Y, first.Value);
            }

            // Difference in value
            var diffVal = next.Value - first.Value;

            // Difference in Y position
            var diffY = next.Point.Y - first.Point.Y;

            // Difference in X position
            var diffX = next.Point.X - first.Point.X;

            var yValue = diffVal / diffY;
            var xValue = diffVal / diffX;

            // Current X position over first item
            var x = touchedXPoint - first.Point.X;

            var points = x * xValue;
            var y = (points / yValue) + first.Point.Y;

            return new Tuple<float, float>(y, points);
        }

        public static string GetCurrentVal(this float touchedXPoint, ChartValueItem first, ChartValueItem next, float points)
        {
            float currentVal = 0;

            if (next != null)
            {
                try
                {
                    currentVal = first.Value + (int)Math.Round(decimal.Parse(points.ToString()), 0, MidpointRounding.AwayFromZero);
                }
                catch (Exception ex)
                {
                    ex.Print();
                }

                if (touchedXPoint == first.Point.X)
                {
                    currentVal = first.Value;
                }
                else if (touchedXPoint == next?.Point.X)
                {
                    currentVal = next.Value;
                }
            }
            else
            {
                currentVal = first.Value;
            }

            return currentVal.ToString();
        }
    }
}
