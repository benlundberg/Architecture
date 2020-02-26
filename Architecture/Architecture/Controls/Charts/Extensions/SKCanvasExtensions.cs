using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Architecture.Controls.Charts
{
    public static class SKCanvasExtensions
    {
        public static void DrawText(this SKCanvas canvas, string text, float x, float y, float textSize, SKColor textColor, bool isBold = false)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = textSize;
                paint.Color = textColor;
                paint.IsAntialias = true;
                paint.FakeBoldText = isBold;

                var bounds = new SKRect();
                paint.MeasureText(text, ref bounds);

                canvas.DrawText(text, x - (bounds.Width / 2), y, paint);
            }
        }
    }
}
