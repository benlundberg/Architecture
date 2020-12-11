using Architecture.Core;
using FFImageLoading;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public class BarChart : BaseChart
    {
        public BarChart()
        {
            this.ChartPadding = new Thickness(4, 0);
            this.HasBackground = false;
            this.InsideFrame = LabelMode.All;
            this.BarMargin = 2f;
            this.GroupMargin = 4f;
            this.MinimumBarWidth = 12f;
            this.BarCornerRadius = 2f;
            this.SliderCornerRadius = 2f;

            ChartType = ChartType.Bar;

            EnableTouchEvents = true;

            this.PaintSurface += BarChart_PaintSurface;

            this.Touch += BarChart_Touch;
        }

        private void BarChart_Touch(object sender, SKTouchEventArgs e)
        {
            if (!(sender is SKCanvasView view))
            {
                return;
            }

            if (!e.InContact)
            {
                return;
            }

            TouchedPoint = e.Location;

            e.Handled = true;
            
            if (!AllowScroll)
            {
                view.InvalidateSurface();
            }
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

            DrawVerticalLabels(canvas, frame, chart);

            if (ChartEntries.Any(x => x.IsVisible))
            {
                CalculateChartValuesXPoints(chart);

                DrawInnerFrame(canvas, frame);
                DrawBackground(canvas, frame);

                DrawHorizontalLabels(canvas, frame, chart);

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    CalculatePoints(entry.Items, frame, chart);
                }

                if (BarsIsVisible)
                {
                    DrawBars(canvas, frame, chart);
                }

                DrawFrame(canvas, frame);
            }
            else
            {
                DrawInnerFrame(canvas, frame);
                DrawFrame(canvas, frame);
            }
        }

        private void DrawBackground(SKCanvas canvas, SKRect frame)
        {
            if (!HasBackground)
            {
                return;
            }
        }

        private void DrawBars(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            // Selected bar width
            var selectedValueItems = ChartEntries.GetChartValueItemFromX(chart.GetInsideXValue(TouchedPoint.X), chart, chart.GetItemWidth(MaxItems));
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

                // Calc max items in one group
                var maxItemsInGroups = groupedItems.Max(x => x.Count());

                // Calc how many groups there is
                var groupCount = groupedItems.Count();

                // Calc how many bars there will be
                var totalBars = groupCount * maxItemsInGroups;

                var internalGroupMargin = maxItemsInGroups == 1 ? BarMargin : GroupMargin;
                var internalBarMargin = maxItemsInGroups == 1 ? 0 : BarMargin;
                var internalBarWidth = MinimumBarWidth;

                float itemWidth;

                if (AllowScroll)
                {
                    // Calc total item width
                    itemWidth = internalBarWidth * maxItemsInGroups + ((maxItemsInGroups - 1) * barMargin);
                }
                else
                {
                    itemWidth = chart.Width / groupCount;

                    itemWidth -= internalGroupMargin;

                    internalBarWidth = (itemWidth / maxItemsInGroups) - ((maxItemsInGroups - 1) * barMargin);
                }

                int groupIndex = 0;

                float scrollLeftPadding = 0;
                float left = 0;
                float groupLeft = 0;
                float right = 0;

                groupCenters = new List<GroupChartItem>();

                foreach (var groupedItem in groupedItems)
                {
                    int itemIndex = 0;

                    groupLeft = left;

                    if (!AllowScroll && IsSliderVisible && selectedTags?.Contains(groupedItem.Key) == true)
                    {
                        var item = groupedItem.OrderByDescending(x => x.Value).First();

                        paint.Color = SliderColor.ToSKColor();
                        paint.PathEffect = SKPathEffect.CreateCorner(SliderCornerRadius);

                        var bounds = new SKRect(
                            item.Point.X - (itemWidth * .5f) - (groupMargin * .5f),
                            item.Point.Y - (itemWidth * .5f),
                            item.Point.X + (itemWidth * .5f) + (groupMargin * .5f),
                            chart.Bottom + HorizontalTextSize);

                        paint.StrokeWidth = 0;

                        canvas.DrawRect(bounds, paint);
                    }

                    foreach (var item in groupedItem)
                    {
                        var parent = ChartEntries.FirstOrDefault(x => x.Items.Contains(item));

                        SKRect bounds = new SKRect();

                        if (AllowScroll)
                        {
                            if (left == 0)
                            {
                                left = itemWidth.FromDpiAdjusted();

                                groupLeft = left;

                                scrollLeftPadding = left;
                            }
                            else if (itemIndex != 0)
                            {
                                left += internalBarMargin;
                            }

                            right = left + internalBarWidth;

                            bounds = new SKRect(
                                left,
                                item.Point.Y,
                                right,
                                chart.Bottom);

                            left = right;
                        }
                        else
                        {
                            left = (item.Point.X - (itemWidth * .5f)) + (internalBarWidth * itemIndex) + (internalBarMargin * itemIndex);
                            right = left + internalBarWidth;

                            bounds = new SKRect(
                                left,
                                item.Point.Y,
                                right,
                                chart.Bottom);
                        }

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

                        itemIndex++;
                    }

                    left += internalGroupMargin;

                    groupIndex++;

                    var groupCenterPosition = groupLeft + (itemWidth / 2);

                    groupCenters.Add(new GroupChartItem
                    {
                        Label = groupedItem.FirstOrDefault().Label,
                        Position = groupCenterPosition.FromDpiAdjusted(),
                        Tag = groupedItem.Key
                    });
                }

                if (AllowScroll)
                {
                    var requestedWidth = right.FromDpiAdjusted() + frame.Left.FromDpiAdjusted();

                    if (requestedWidth != this.WidthRequest)
                    {
                        this.WidthRequest = requestedWidth;
                    }
                    else
                    {
                        SetStartPosition(groupCenters);
                        IsInitialized = true;
                    }

                    if (ScrollComponent != null)
                    {
                        var deviceWidth = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;

                        var halfDeviceWidth = ((float)(deviceWidth / 2)).FromDpiAdjusted();

                        var frameLeftVal = frame.Left.FromDpiAdjusted();
                        var rectRightMarginVal = ChartRectMargin.Right.FromDpiAdjusted();

                        var leftPadding = halfDeviceWidth - frameLeftVal - scrollLeftPadding.FromDpiAdjusted();

                        var rightPadding = halfDeviceWidth - frameLeftVal - rectRightMarginVal - itemWidth.FromDpiAdjusted();

                        ScrollComponent.Padding = new Thickness(leftPadding, 0, rightPadding, 0);
                        ScrollComponent.Margin = new Thickness(frame.Left.FromDpiAdjusted(), 0, ChartRectMargin.Right.FromDpiAdjusted(), 0);

                        if (Selector != null)
                        {
                            Selector.WidthRequest = itemWidth.FromDpiAdjusted() + 8;
                            Selector.Margin = new Thickness(halfDeviceWidth - 4, 0, 0, this.Height - frame.Bottom.FromDpiAdjusted());
                        }

                        if (SelectorLabel != null)
                        {
                            SelectorLabel.Margin = new Thickness(Selector.Margin.Left - ((SelectorLabel.Width - Selector.WidthRequest) / 2), frame.Bottom.FromDpiAdjusted(), 0, 0);

                            if (SelectorLabel.Width < Selector.Width)
                            {
                                SelectorLabel.WidthRequest = Selector.Width;
                            }
                            else
                            {
                                SelectorLabel.WidthRequest = -1;
                            }

                            if (!isLabelSizeEventActive)
                            {
                                isLabelSizeEventActive = true;
                                SelectorLabel.SizeChanged += SelectorLabel_SizeChanged;
                            }
                        }

                        if (!isScrollEventActive)
                        {
                            ScrollComponent.Scrolled += ScrollComponent_Scrolled;
                            isScrollEventActive = true;
                        }
                    }

                    if (SwipeNotificationLeft != null)
                    {
                        SwipeNotificationLeft.Margin = new Thickness(frame.Left.FromDpiAdjusted(), 0, 0, this.Height - frame.Bottom.FromDpiAdjusted());
                        SwipeNotificationLeft.WidthRequest = ScrollComponent.Padding.Left;
                    }

                    if (SwipeNotificationRight != null)
                    {
                        SwipeNotificationRight.Margin = new Thickness(0, 0, ChartRectMargin.Right.FromDpiAdjusted(), this.Height - frame.Bottom.FromDpiAdjusted());
                        SwipeNotificationRight.WidthRequest = ScrollComponent.Padding.Left;
                    }
                }
            }

            DrawHorizontalLabel(selectedValueItems?.FirstOrDefault()?.ChartValueItem, canvas, frame, chart);
        }

        private void SelectorLabel_SizeChanged(object sender, EventArgs e)
        {
            double margin = Selector.Margin.Left - ((SelectorLabel.Width - Selector.WidthRequest) / 2);
            var abs = Math.Truncate(SelectorLabel.Margin.Left);

            if (abs == Math.Truncate(margin))
            {
                return;
            }
            else
            {
                SelectorLabel.Margin = new Thickness(margin, SelectorLabel.Margin.Top, 0, 0);
            }

            if (SelectorLabel.WidthRequest != Selector.Width)
            {
                if (SelectorLabel.Width < Selector.Width)
                {
                    SelectorLabel.WidthRequest = Selector.Width;
                }
            }
        }

        private void ShowSwipeNotifications(double currentPosition)
        {
            if (!SwipeNotificationIsDisabled)
            {
                if (currentPosition <= ScrollComponent.Margin.Right)
                {
                    if (SwipeNotificationRight.Opacity > 0)
                    {
                        SwipeNotificationRight.Opacity = 0;
                        SwipeNotificationRight.InputTransparent = true;
                    }

                    if (SwipeNotificationLeft.Opacity > 0 && SwipePreviousIsDisabled)
                    {
                        SwipeNotificationLeft.Opacity = 0;
                        SwipeNotificationLeft.InputTransparent = true;
                    }

                    if (SwipeNotificationLeft.Opacity == 0 && !SwipePreviousIsDisabled)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await SwipeNotificationLeft.FadeTo(1);
                            SwipeNotificationLeft.InputTransparent = false;
                        });
                    }
                }
                else if (currentPosition >= (groupCenters.OrderByDescending(x => x.Position).FirstOrDefault().Position - ScrollComponent.Margin.Left))
                {
                    if (SwipeNotificationLeft.Opacity > 0)
                    {
                        SwipeNotificationLeft.Opacity = 0;
                        SwipeNotificationLeft.InputTransparent = true;
                    }

                    if (SwipeNotificationRight.Opacity > 0 && SwipeNextIsDisabled)
                    {
                        SwipeNotificationRight.Opacity = 0;
                        SwipeNotificationRight.InputTransparent = true;
                    }

                    if (SwipeNotificationRight.Opacity == 0 && !SwipeNextIsDisabled)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await SwipeNotificationRight.FadeTo(1);
                            SwipeNotificationRight.InputTransparent = false;
                        });
                    }
                }
                else
                {
                    SwipeNotificationLeft.Opacity = 0;
                    SwipeNotificationRight.Opacity = 0;
                    SwipeNotificationLeft.InputTransparent = true;
                    SwipeNotificationRight.InputTransparent = true;
                }
            }
            else
            {
                SwipeNotificationLeft.Opacity = 0;
                SwipeNotificationRight.Opacity = 0;
                SwipeNotificationLeft.InputTransparent = true;
                SwipeNotificationRight.InputTransparent = true;
            }
        }

        private void ScrollComponent_Scrolled(object sender, ScrolledEventArgs e)
        {
            ShowSwipeNotifications(e.ScrollX);

            if (isSnapping)
            {
                return;
            }

            if (lastScrollPosition == e.ScrollX)
            {
                return;
            }

            lastScrollPosition = e.ScrollX;

            SwitchSelectedLabel();

            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
            {
                if (lastScrollPosition == e.ScrollX)
                {
                    var entries = groupCenters.OrderByDescending(x => x.Position);

                    var firstItemsPosition = entries.LastOrDefault().Position;

                    var item = entries.FirstOrDefault(x => Math.Round((decimal)x.Position - (decimal)(firstItemsPosition / 2), 0) <= Math.Round((decimal)(lastScrollPosition + firstItemsPosition)));

                    if (isSnapping)
                    {
                        return false;
                    }

                    if (item == null)
                    {
                        return false;
                    }

                    SnapToClosestBar(item.Position - firstItemsPosition, item);
                }

                return false;
            });
        }

        private void SnapToClosestBar(double position, GroupChartItem selectedItem)
        {
            var selectedValues = new SelectedChartValueItemArgs
            {
                ChartValueItems = new List<ChartValueItemParam>(),
                TouchedPoint = TouchedPoint
            };

            foreach (var chartEntry in ChartEntries.Where(x => x.IsVisible))
            {
                var chartEntryItem = chartEntry.Items.FirstOrDefault(i => i.Tag.ToString() == selectedItem.Tag.ToString());

                if (chartEntryItem == null)
                {
                    continue;
                }

                selectedValues.ChartValueItems.Add(new ChartValueItemParam(chartEntryItem, null, chartEntry));
            }

            // Invoke command with selected items
            SelectedValuesCommand?.Execute(selectedValues);

            Device.BeginInvokeOnMainThread(async () =>
            {
                isSnapping = true;

                ScrollComponent.Scrolled -= ScrollComponent_Scrolled;

                SelectedLabel = selectedItem.Label;

                lastScrollPosition = position;

                await ScrollComponent.ScrollToAsync(lastScrollPosition, 0, false);

                ShowSwipeNotifications(lastScrollPosition);

                ScrollComponent.Scrolled += ScrollComponent_Scrolled;

                isSnapping = false;
            });
        }

        private void SetStartPosition(List<GroupChartItem> groupCenters)
        {
            GroupChartItem item = null;
            var entries = groupCenters.OrderByDescending(x => x.Position);

            if (StartPositionEnd == false)
            {
                item = entries.LastOrDefault();
                StartPositionEnd = null;
            }
            else if (StartPositionEnd == true)
            {
                item = entries.FirstOrDefault();
                StartPositionEnd = null;
            }
            else if (!string.IsNullOrEmpty(SelectedTag) && entries.Any(x => x.Label == SelectedLabel))
            {
                item = entries.FirstOrDefault(x => x.Tag == SelectedTag);
            }

            if (item == null)
            {
                item = entries.ElementAtOrDefault(entries.Count() / 2);
            }

            var firstItemsPosition = entries.LastOrDefault().Position;

            var position = item.Position - firstItemsPosition;

            ShowSwipeNotifications(position);

            SnapToClosestBar(position, item);
        }

        private void SwitchSelectedLabel()
        {
            try
            {
                // When user scrolling we switch selected label
                var entries = groupCenters.OrderByDescending(x => x.Position);

                var firstItemsPosition = entries.LastOrDefault().Position;

                var item = entries.FirstOrDefault(x => Math.Round((decimal)x.Position - (decimal)(firstItemsPosition / 2), 0) <= Math.Round((decimal)(lastScrollPosition + firstItemsPosition)));

                SelectedLabel = item?.Label;
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        private ICommand swipeRightCommand;
        public ICommand SwipeRightCommand => swipeRightCommand ?? (swipeRightCommand = new Command(() =>
        {
            App.Current.MainPage.DisplayAlert("Swipe right", "You swiped right", "Ok");
        }));

        private ICommand swipeLeftCommand;
        public ICommand SwipeLeftCommand => swipeLeftCommand ?? (swipeLeftCommand = new Command(() =>
        {
            App.Current.MainPage.DisplayAlert("Swipe left", "You swiped left", "Ok");
        }));

        public bool BarsIsVisible { get; set; } = true;

        private float barMargin;
        public float BarMargin
        {
            get { return barMargin; }
            set { barMargin = value.ToDpiAdjusted(); }
        }

        private float groupMargin;
        public float GroupMargin
        {
            get { return groupMargin; }
            set { groupMargin = value.ToDpiAdjusted(); }
        }

        private float minimumBarWidth;
        public float MinimumBarWidth
        {
            get { return minimumBarWidth; }
            set { minimumBarWidth = value.ToDpiAdjusted(); }
        }

        private float barCornerRadius;
        public float BarCornerRadius
        {
            get { return barCornerRadius; }
            set { barCornerRadius = value.ToDpiAdjusted(); }
        }

        private float sliderCornerRadius;
        public float SliderCornerRadius
        {
            get { return sliderCornerRadius; }
            set { sliderCornerRadius = value.ToDpiAdjusted(); }
        }

        public string SelectedLabel { get; set; }
        public bool? StartPositionEnd { get; set; }

        public bool AllowScroll { get; set; }

        private double lastScrollPosition;
        private bool isScrollEventActive;
        private bool isSnapping;
        private bool isLabelSizeEventActive;

        private List<GroupChartItem> groupCenters;

        public ScrollView ScrollComponent { get; set; }
        public BoxView Selector { get; set; }
        public Frame SelectorLabel { get; set; }
        public Grid SwipeNotificationLeft { get; set; }
        public Grid SwipeNotificationRight { get; set; }

        public static readonly BindableProperty SwipeNotificationIsDisabledProperty = BindableProperty.Create(
            "SwipeNotificationIsDisabled",
            typeof(bool),
            typeof(BarChart),
            false);

        public bool SwipeNotificationIsDisabled
        {
            get { return (bool)GetValue(SwipeNotificationIsDisabledProperty); }
            set { SetValue(SwipeNotificationIsDisabledProperty, value); }
        }

        public static readonly BindableProperty SwipeNextIsDisabledProperty = BindableProperty.Create(
            "SwipeNextIsDisabled",
            typeof(bool),
            typeof(BarChart),
            false);

        public bool SwipeNextIsDisabled
        {
            get { return (bool)GetValue(SwipeNextIsDisabledProperty); }
            set { SetValue(SwipeNextIsDisabledProperty, value); }
        }

        public static readonly BindableProperty SwipePreviousIsDisabledProperty = BindableProperty.Create(
            "SwipePreviousIsDisabled",
            typeof(bool),
            typeof(BarChart),
            false);

        public bool SwipePreviousIsDisabled
        {
            get { return (bool)GetValue(SwipePreviousIsDisabledProperty); }
            set { SetValue(SwipePreviousIsDisabledProperty, value); }
        }
    }

    public class GroupChartItem
    {
        public double Position { get; set; }
        public string Label { get; set; }
        public string Tag { get; set; }
    }
}
