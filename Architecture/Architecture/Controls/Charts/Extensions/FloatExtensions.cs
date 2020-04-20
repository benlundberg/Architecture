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
                    y = yPosition + (textSize / 2);
                }
                else if (textAlignment == TextAlignment.End)
                {
                    y = yPosition + textSize;
                }
                else
                {
                    y = yPosition;
                }
            }

            return y;
        }

        public static string ToRoundedString(this float value)
        {
            return decimal.TryParse(value.ToString(), out decimal res) ? Math.Round(res, 0, MidpointRounding.AwayFromZero).ToString() : "0";
        }

        public static float ToRounded(this float value, int decimals = 0)
        {
            return (float)(decimal.TryParse(value.ToString(), out decimal res) ? Math.Round(res, decimals, MidpointRounding.AwayFromZero) : 0);
        }
    }
}
