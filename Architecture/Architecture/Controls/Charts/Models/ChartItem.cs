using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public class ChartItem
    {
        public int Id { get; set; }
        public float LineWidth { get; set; } = Device.RuntimePlatform == Device.Android ? 10f : 6f;
        public float PointSize { get; set; } = Device.RuntimePlatform == Device.Android ? 32f : 22f;
        public SKStrokeCap StrokeCap { get; set; } = SKStrokeCap.Round;
        public Color Color { get; set; } = Color.Black;
        public Color PointColor { get; set; } = Color.Black;
        public IList<ChartValueItem> Items { get; set; }
        public bool UseDashedEffect { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsPointsVisible { get; set; }
    }
}
