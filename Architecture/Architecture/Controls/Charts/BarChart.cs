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

            EnableTouchEvents = true;

            this.PaintSurface += BarChart_PaintSurface;
            //this.Touch += BarChart_Touch;
        }

        private void BarChart_Touch(object sender, SKTouchEventArgs e)
        {
            if (!(sender is SKCanvasView view))
            {
                return;
            }

            TouchedPoint = e.Location;

            e.Handled = true;

            view.InvalidateSurface();
        }

        private void BarChart_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
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
            DrawVerticalLabels(canvas, frame, chart);

            if (ChartEntries.Any(x => x.IsVisible))
            {
                CalculateChartValuesXPoints(chart);

                DrawHorizontalLabels(canvas, frame, chart);

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    CalculatePoints(entry.Items, frame, chart);
                }

                if (BarsIsVisible)
                {
                    DrawBars(canvas, frame, chart);
                }
            }
        }

        private void DrawBars(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            var itemWidth = BarWidth; //(chart.Width / (MaxItems < 10 ? 10 : MaxItems)) * 0.8f;

            // Selected bar width
            var selectedValueItems = ChartEntries.GetChartValueItemFromX(chart.GetInsideXValue(TouchedPoint.X), chart, chart.GetItemWidth(MaxItems), false, BlockStartIndex, BlockCount);
            var selectedTags = selectedValueItems?.Select(x => x.ChartValueItem.Tag);

            // Invoke command with selected items
            SelectedValuesCommand?.Execute(new SelectedChartValueItemArgs
            {
                ChartValueItems = selectedValueItems,
                TouchedPoint = new SKPoint(chart.GetInsideXValue(TouchedPoint.X), TouchedPoint.Y)
            });

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeCap = SKStrokeCap.Butt,
            })
            {
                var groupedItems = ChartEntries.Where(x => x.IsVisible).SelectMany(x => x.Items).GroupBy(x => x.Tag.ToString()).OrderBy(x => x.Key);

                if (BlockCount > 0)
                {
                    groupedItems = groupedItems.Skip(BlockStartIndex).Take(BlockCount).OrderBy(x => x.Key);
                }

                foreach (var groupedItem in groupedItems)
                {
                    int index = 0;

                    if (IsSliderVisible && selectedTags?.Contains(groupedItem.Key) == true)
                    {
                        var item = groupedItem.OrderByDescending(x => x.Value).First();

                        paint.Color = SliderColor.ToSKColor();
                        paint.PathEffect = SKPathEffect.CreateCorner(SliderCornerRadius);

                        var bounds = new SKRect(
                            item.Point.X - (itemWidth * .8f),
                            item.Point.Y - (itemWidth * .8f),
                            item.Point.X + (itemWidth * .8f),
                            chart.Bottom + (HorizontalTextSize * 2));

                        paint.StrokeWidth = 0;

                        canvas.DrawRect(bounds, paint);
                    }

                    foreach (var item in groupedItem)
                    {
                        var parent = ChartEntries.FirstOrDefault(x => x.Items.Contains(item));

                        var barWidth = itemWidth / groupedItem.Count();

                        float left = frame.GetInsideXValue((item.Point.X - itemWidth) + (barWidth * index) + (itemWidth / 2));

                        float right = frame.GetInsideXValue(left + barWidth);

                        var bounds = new SKRect(
                            left + BarMargin,
                            item.Point.Y,
                            right - BarMargin,
                            chart.Bottom);

                        paint.StrokeWidth = 0;
                        
                        if (parent.UseDashedEffect)
                        {
                            paint.Color = parent.Color.ToSKColor().AsTransparency();
                            paint.PathEffect = SKPathEffect.CreateCorner(BarCornerRadius);

                            canvas.DrawRect(bounds, paint);

                            paint.Color = parent.Color.ToSKColor();
                            paint.PathEffect = SKPathEffect.CreateDash(new float[] { StrokeDashFirst, StrokeDashSecond }, StrokeDashPhase);
                            
                            paint.StrokeWidth = bounds.Width;

                            canvas.DrawLine(bounds.MidX, bounds.Bottom, bounds.MidX, bounds.Top, paint);
                        }
                        else
                        {
                            paint.Color = parent.Color.ToSKColor(); 
                            paint.PathEffect = SKPathEffect.CreateCorner(BarCornerRadius);
                            canvas.DrawRect(bounds, paint);
                        }

                        index++;
                    }
                }
            }

            DrawHorizontalLabel(selectedValueItems?.FirstOrDefault()?.ChartValueItem, canvas, frame, chart);
        }

        public bool BarsIsVisible { get; set; } = true;
        public int BarMargin { get; set; }
        public int BarWidth { get; set; } = 50;
        public float BarCornerRadius { get; set; } = 10f;
        public float SliderCornerRadius { get; set; } = 14f;
    }
}
