using SkiaSharp;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public static class SKCanvasExtensions
    {
        public static void DrawHorizontalText(this SKCanvas canvas, string text, float x, float y, float textSize, SKColor textColor, SKTextAlign textAlign, bool isBold = false)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = textSize;
                paint.Color = textColor;
                paint.IsAntialias = true;
                paint.FakeBoldText = isBold;

                var bounds = new SKRect();
                paint.MeasureText(text, ref bounds);

                if (textAlign == SKTextAlign.Left)
                {
                    x -= bounds.Width;
                }
                else if (textAlign == SKTextAlign.Right)
                {
                    x += bounds.Width / 2;
                }
                else
                {
                    paint.TextAlign = SKTextAlign.Center;
                }

                canvas.DrawText(text, x, y, paint);
            }
        }

        public static void DrawSliderCircle(this SKCanvas canvas, float x, float y, SKColor color, float pointSize)
        {
            using (var paint = new SKPaint
            {
                Color = color,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = pointSize
            })
            {
                canvas.DrawCircle(new SKPoint(x, y), pointSize, paint);
            }
        }

        public static void DrawSliderValue(this SKCanvas canvas, string text, float x, float y, float textSize, SKColor textColor, SKColor backgroundColor, int padding, string maxText, int count, int index, StackOrientation orientation, SKRect frame, bool useDashedEffect)
        {
            using (var textPaint = new SKPaint())
            {
                textPaint.TextSize = textSize;
                textPaint.Color = textColor;
                textPaint.IsAntialias = true;
                textPaint.StrokeCap = SKStrokeCap.Round;

                // Calculates the longest possible text
                SKRect textBounds = new SKRect();
                textPaint.MeasureText(maxText, ref textBounds);

                // Sets the width and height of the view
                var width = textBounds.Width + padding;
                var height = textBounds.Height + padding;

                float xPosition, yPosition;

                if (orientation == StackOrientation.Vertical)
                {
                    if (x - (width / 2) < frame.Left)
                    {
                        xPosition = frame.Left;
                    }
                    else if (x + (width / 2) > frame.Right)
                    {
                        xPosition = frame.Right - width;
                    }
                    else
                    {
                        xPosition = x - (width / 2);
                    }

                    yPosition = y + (height * index);
                }
                else
                {
                    var totalWidth = width * count;

                    if (totalWidth / 2 >= x - frame.Left)
                    {
                        xPosition = x + (index * width);
                    }
                    else if (totalWidth / 2 >= frame.Right - x)
                    {
                        xPosition = x - totalWidth + (index * width);
                    }
                    else
                    {
                        xPosition = x - ((totalWidth / 2) - (index * width));
                    }

                    yPosition = y;
                }

                using (var backgroundPaint = new SKPaint())
                {
                    backgroundPaint.Color = backgroundColor;
                    backgroundPaint.StrokeCap = SKStrokeCap.Round;

                    if (useDashedEffect)
                    {
                        backgroundPaint.PathEffect = SKPathEffect.CreateDash(new float[] { 12, 12 }, 0);
                        backgroundPaint.Style = SKPaintStyle.Stroke;
                        backgroundPaint.StrokeWidth = 8f;
                        backgroundPaint.IsStroke = true;

                        textPaint.Color = backgroundColor;
                    }
                    else
                    {
                        backgroundPaint.Style = SKPaintStyle.StrokeAndFill;
                        backgroundPaint.StrokeWidth = 3f;
                    }

                    SKRect background = new SKRect(xPosition, yPosition, xPosition + width, yPosition + height);
                    canvas.DrawRect(background, backgroundPaint);

                    SKRect finalTextBounds = new SKRect();
                    textPaint.MeasureText(text, ref finalTextBounds);

                    canvas.DrawText(text, background.MidX - (finalTextBounds.Width / 2), background.MidY + (finalTextBounds.Height / 2), textPaint);
                }
            }
        }
    }
}
