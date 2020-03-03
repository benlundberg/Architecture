using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Linq;

namespace Architecture.Controls.Charts
{
    public class BarChart : BaseChart
    {
        public BarChart()
        {
            ChartType = ChartType.Bar;
            ChartPadding = new SKRect(40, 0, 40, 0);

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

            canvas.Clear();

            var frame = CreateFrame(info);
            var chart = CreateChart(frame);

            DrawFrame(canvas, frame);

            if (ChartEntries.Any(x => x.IsVisible))
            {
                CalculateChartValuesXPoints(chart);

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    CalculatePoints(entry.Items, frame, chart);
                }

                DrawBars(canvas, frame, chart);
            }

            DrawVerticalLabels(canvas, frame, chart);
            DrawHorizontalLabels(canvas, frame, chart);
        }

        private void DrawBars(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            var itemWidth = chart.GetItemWidth(MaxItems);
            var count = ChartEntries.Count(x => x.IsVisible);

            // Regular bar width
            var barWidth = itemWidth / count - 10;
            var totalBarWidth = barWidth * count;

            // Selected bar width
            var selectedValueItems = ChartEntries.GetChartValueItemFromX(chart.GetInsideXValue(TouchedPoint.X), chart);

            var selectedBarWidth = itemWidth / selectedValueItems?.Count() ?? 1;
            var totalSelectedBarWidth = selectedBarWidth * selectedValueItems?.Count() ?? 1;

            int index = 0;

            foreach (var item in ChartEntries.Where(x => x.IsVisible))
            {
                foreach (var valueItem in item.Items)
                {
                    using (var paint = new SKPaint())
                    {
                        using (var path = new SKPath())
                        {
                            paint.StrokeCap = SKStrokeCap.Square;
                            paint.Style = SKPaintStyle.Stroke;
                            paint.IsStroke = true;
                            paint.IsAntialias = true;

                            if (IsSliderVisible && selectedValueItems?.Select(x => x.ChartValueItem.Tag)?.Contains(valueItem.Tag) == true)
                            {
                                // Selected item //

                                if (item.UseDashedEffect)
                                {
                                    paint.PathEffect = SKPathEffect.CreateDash(new float[] { 0, (float)((selectedBarWidth / 2) * Math.PI) }, 0);
                                }

                                paint.Color = item.Color;
                                paint.StrokeWidth = selectedBarWidth;

                                var selectedMiddle = totalSelectedBarWidth / 2;

                                path.MoveTo(valueItem.Point.X - (selectedMiddle - (index * selectedBarWidth)), frame.Bottom - (paint.StrokeWidth / 2));
                                path.LineTo(valueItem.Point.X - (selectedMiddle - (index * selectedBarWidth)), valueItem.Point.Y + (paint.StrokeWidth / 2));

                                canvas.DrawPath(path, paint);

                                string text = Math.Round(double.Parse(valueItem.Value.ToString()), 0, MidpointRounding.AwayFromZero).ToString() + " " + this.VerticalUnit;

                                canvas.DrawSliderValue(
                                    text, 
                                    frame.GetInsideXValue(valueItem.Point.X), 
                                    frame.Top, 
                                    SliderDetailTextSize, 
                                    SKColors.White,
                                    item.Color,
                                    SliderDetailPadding, 
                                    SliderDetailMargin, 
                                    MaxValue + " " + this.VerticalUnit, 
                                    selectedValueItems.Count, 
                                    index, 
                                    SliderDetailOrientation, 
                                    frame, 
                                    item.UseDashedEffect);
                            }
                            else
                            {
                                if (item.UseDashedEffect)
                                {
                                    paint.PathEffect = SKPathEffect.CreateDash(new float[] { 0, (float)((barWidth / 2) * Math.PI) }, 10);
                                }

                                paint.Color = IsSliderVisible ? item.Color.AsTransparency() : item.Color;
                                paint.StrokeWidth = barWidth;

                                var middle = totalBarWidth / 2;

                                path.MoveTo(valueItem.Point.X - (middle - (index * barWidth)), frame.Bottom - (paint.StrokeWidth / 2));
                                path.LineTo(valueItem.Point.X - (middle - (index * barWidth)), valueItem.Point.Y + (paint.StrokeWidth / 2));

                                canvas.DrawPath(path, paint);
                            }
                        }
                    }
                }

                index++;
            }

            if (!IsSliderVisible)
            {
                return;
            }

            float hintX = selectedValueItems?.FirstOrDefault()?.ChartValueItem?.Point.X ?? 0;
            float hintY = selectedValueItems?.OrderByDescending(x => x.ChartValueItem.Point.Y)?.FirstOrDefault()?.ChartValueItem?.Point.Y ?? 0;

            if (hintX != 0 && hintY != 0)
            {
                DrawDragHintGraphic(
                    canvas,
                    hintX,
                    hintY + ((frame.Bottom - hintY) / 2),
                    totalSelectedBarWidth,
                    frame);
            }
        }
    }
}
