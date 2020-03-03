using SkiaSharp;

namespace Architecture.Controls.Charts
{
    public class ChartValueItem
    {
        public float Value { get; set; }
        public string Label { get; set; }
        public SKPoint Point { get; set; }
        public object Tag { get; set; }
    }
}
