using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public class ChartItem
    {
        public int Id { get; set; }

        private float? lineWidth;
        public float LineWidth 
        { 
            get { return lineWidth == null ? (4f).ToDpiAdjusted() : ((float)lineWidth).ToDpiAdjusted(); } 
            set { lineWidth = value; }
        }

        private float? pointSize;
        public float PointSize 
        { 
            get { return pointSize == null ? (8f).ToDpiAdjusted() : ((float)pointSize).ToDpiAdjusted(); } 
            set { pointSize = value; }
        }
        
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
