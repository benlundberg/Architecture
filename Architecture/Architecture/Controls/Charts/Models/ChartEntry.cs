using SkiaSharp;
using System.Collections.Generic;

namespace Architecture.Controls.Charts
{
    public class ChartEntry
    {
        public int Id { get; set; }
        public float LineWidth { get; set; } = 10f;
        public float PointSize { get; set; } = 32f;
        public SKStrokeCap StrokeCap { get; set; } = SKStrokeCap.Round;
        public SKColor Color { get; set; } = SKColors.Black;
        public SKColor PointColor { get; set; } = SKColors.Black;
        public IList<ChartEntryItem> Items { get; set; }
        public IList<ChartEntryItem> CompareItems { get; set; }

        public bool IsVisible { get; set; } = true;
        public bool IsPointsVisible { get; set; }
    }
}
