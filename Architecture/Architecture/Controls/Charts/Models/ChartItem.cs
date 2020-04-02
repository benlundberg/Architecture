using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public class ChartItem
    {
        public int Id { get; set; }
        public float LineWidth { get; set; } = 10f;
        public float PointSize { get; set; } = 32f;
        public SKStrokeCap StrokeCap { get; set; } = SKStrokeCap.Round;
        public Color Color { get; set; } = Color.Black;
        public Color PointColor { get; set; } = Color.Black;
        public Color TextColor { get; set; } = Color.White;
        public IList<ChartValueItem> Items { get; set; }
        public bool UseDashedEffect { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsPointsVisible { get; set; }
    }
}
