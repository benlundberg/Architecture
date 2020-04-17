using SkiaSharp;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public class ChartValueItem
    {
        public float Value { get; set; }
        public string Label { get; set; }
        public SKPoint Point { get; set; }
        public string Tag { get; set; }
        public Color Color { get; set; }
        public bool UseDashedEffect { get; set; }
    }
}
