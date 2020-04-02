using SkiaSharp;
using System;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public static class SKCanvasExtensions
    {
        public static void DrawBar(this SKCanvas canvas, float x, float yStart, float yEnd, float width, int margin, int count, int index, SKColor color, bool useDashedEffect)
        {
            using (var paint = new SKPaint())
            {
                using (var path = new SKPath())
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeCap = SKStrokeCap.Butt;
                    paint.IsStroke = true;
                    paint.IsAntialias = true;
                    paint.Color = color;
                    paint.StrokeWidth = width;

                    if (useDashedEffect)
                    {
                        paint.PathEffect = SKPathEffect.CreateDash(new float[] { width, width }, width);
                    }

                    var middle = ((width * (count - 1)) + (margin * count)) / 2;

                    path.MoveTo(x - (middle - (index * width)) + (margin * index), yStart);
                    path.LineTo(x - (middle - (index * width)) + (margin * index), yEnd);

                    canvas.DrawPath(path, paint);
                }
            }
        }
    }
}
