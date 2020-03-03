using SkiaSharp;
using System;

namespace Architecture.Controls.Charts
{
    public static class SKColorExtensions
    {
        public static SKColor AsTransparency(this SKColor color)
        {
            var transparency = 0.5 * (1 + Math.Sin(1 * 2 * Math.PI));
            return color.WithAlpha((byte)(0xFF * (1 - transparency)));
        }
    }
}
