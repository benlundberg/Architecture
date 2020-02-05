using SkiaSharp;
using System.Collections.Generic;

namespace Architecture.Controls
{
    public class ChartEntry
    {
        public bool IsVisible { get; set; }
        public float LineWidth { get; set; } = 4f;
        public SKColor Color { get; set; } = SKColors.Black;
        public SKColor PointColor { get; set; } = SKColors.Black;
        public IList<ChartEntryItem> Items { get; set; }
        public SKPath Path { get; set; }
    }
}
