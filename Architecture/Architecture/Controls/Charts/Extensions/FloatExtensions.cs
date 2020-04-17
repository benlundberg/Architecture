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
    }
}
