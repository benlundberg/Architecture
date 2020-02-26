using Architecture.Core;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Linq;

namespace Architecture.Controls.Charts
{
    public enum LineMode
    {
        Straight,
        Spline
    }

    public class LineChart : Chart
    {
        public LineChart()
        {
            ChartMargin = new SKRect(20, 100, 40, 20);

            EnableTouchEvents = true;

            this.PaintSurface += LineChart_PaintSurface;
            this.Touch += LineChart_Touch;
        }

        private void LineChart_Touch(object sender, SKTouchEventArgs e)
        {
            if (!(sender is SKCanvasView view))
            {
                return;
            }

            TouchedPoint = e.Location;

            e.Handled = true;

            view.InvalidateSurface();
        }

        private void LineChart_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (ChartEntries?.Any() != true)
            {
                return;
            }

            var info = e.Info;
            var canvas = e.Surface.Canvas;

            // Calculate size of vertical/left labels
            var verticalLabelSize = MeasureVerticalLabels();

            // Calculate left width where vertical labels will be
            var leftPadding = CalculateLeftWidth(verticalLabelSize);

            // Calculate footer height where horizontal labels will be
            var footerHeight = CalculateFooterHeight();

            // Create a rectangle of the frame
            var frameRect = new SKRect(
                left: leftPadding,
                top: ChartMargin.Top,
                right: info.Width - ChartMargin.Right,
                bottom: info.Height - footerHeight);

            // Create a rectangle of the chart inside the frame
            var chartRect = new SKRect(
                left: leftPadding + ChartPadding.Left,
                top: ChartMargin.Top + ChartPadding.Top,
                right: info.Width - ChartMargin.Right - ChartPadding.Right,
                bottom: info.Height - footerHeight - ChartMargin.Bottom);

            canvas.Clear();

            // Draw the frame on the canvas
            DrawFrame(canvas, frameRect);

            if (ChartEntries.Any(x => x.IsVisible))
            {
                // First calculate all x values by label to know where item should be according to date
                CalculateXPoints(chartRect);

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    var points = CalculatePoints(entry, frameRect, chartRect);

                    // Draw lines to the canvas, based on calculated points
                    DrawLines(entry, canvas, points, frameRect.Width);

                    DrawArea(canvas, points);

                    // Draws point if visible on every items values
                    if (entry.IsPointsVisible)
                    {
                        DrawPoints(canvas, points, entry);
                    }
                }
            }

            DrawHorizontalLabels(canvas, frameRect, info.Height - (footerHeight / 2));

            DrawVerticalLabels(canvas, frameRect, chartRect);

            DrawSlider(canvas, frameRect);

            try
            {
                GetValueFromPosition(canvas, frameRect);
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        private void DrawLines(ChartEntry chartEntry, SKCanvas canvas, SKPoint[] points, float width)
        {
            if (points?.Any() != true)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = chartEntry.Color,
                StrokeWidth = chartEntry.LineWidth,
                IsAntialias = true,
            })
            {

                var path = new SKPath();

                path.MoveTo(points.First());

                var last = LineMode == LineMode.Straight ? points.Length : points.Length - 1;

                for (int i = 0; i < last; i++)
                {
                    if (LineMode == LineMode.Spline)
                    {
                        var total = chartEntry.Items.Count;

                        var w = (width - ((total + 1) * this.ChartMargin.Left)) / total;

                        var (control, nextPoint, nextControl) = this.CalculateCubicInfo(points, i, w);

                        path.CubicTo(control, nextControl, nextPoint);
                    }
                    else
                    {
                        path.LineTo(points[i]);
                    }
                }

                canvas.DrawPath(path, paint);
            }
        }

        private (SKPoint control, SKPoint nextPoint, SKPoint nextControl) CalculateCubicInfo(SKPoint[] points, int i, float width)
        {
            var point = points[i];
            var nextPoint = points[i + 1];
            var controlOffset = new SKPoint(width * 0.8f, 0);
            var currentControl = point + controlOffset;
            var nextControl = nextPoint - controlOffset;
            return (currentControl, nextPoint, nextControl);
        }

        private SKShader CreateGradient(SKPoint[] points, byte alpha = 255)
        {
            var startX = points.First().X;
            var endX = points.Last().X;
            var rangeX = endX - startX;

            return SKShader.CreateLinearGradient(
                new SKPoint(startX, 0),
                new SKPoint(endX, 0),
                this.ChartEntries.Select(x => x.Color.WithAlpha(alpha)).ToArray(),
                null,
                SKShaderTileMode.Clamp);
        }

        protected void DrawArea(SKCanvas canvas, SKPoint[] points)
        {
            if (this.LineAreaAlpha > 0 && points.Length > 1)
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.White,
                    IsAntialias = true,
                })
                {
                    using (var shader = this.CreateGradient(points, this.LineAreaAlpha))
                    {
                        paint.Shader = shader;

                        var path = new SKPath();

                        path.MoveTo(points.First());
                        path.LineTo(points.First());

                        var last = (this.LineMode == LineMode.Spline) ? points.Length - 1 : points.Length;

                        for (int i = 0; i < last; i++)
                        {
                            if (this.LineMode == LineMode.Straight)
                            {
                                path.LineTo(points[i]);
                            }
                        }

                        path.LineTo(points.Last());

                        path.Close();

                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        public byte LineAreaAlpha { get; set; } = 32;
    }
}
