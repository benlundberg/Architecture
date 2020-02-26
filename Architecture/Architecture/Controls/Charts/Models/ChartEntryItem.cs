using SkiaSharp;

namespace Architecture.Controls.Charts
{
    public class ChartEntryItem
    {
        public float Value { get; set; }
        public string Label { get; set; }
        public SKPoint Point { get; set; }
        public string Tag { get; set; }
    }
}
