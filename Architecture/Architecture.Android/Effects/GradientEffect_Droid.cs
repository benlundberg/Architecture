using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

[assembly: ResolutionGroupName("Architecture")]
[assembly: ExportEffect(typeof(Architecture.Droid.GradientEffect_Droid), nameof(Architecture.GradientEffect))]
namespace Architecture.Droid
{
    public class GradientEffect_Droid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var view = Control ?? Container;

                var effect = Element.Effects.OfType<GradientEffect>()?.FirstOrDefault(e => e is GradientEffect);

                if (effect == null)
                {
                    return;
                }

                var colors = new int[]
                {
                    effect.StartColor.ToAndroid().ToArgb(),
                    effect.EndColor.ToAndroid().ToArgb()
                };

                var paint = new PaintDrawable
                {
                    Shape = new RectShape()
                };

                paint.SetShaderFactory(new GradientShaderFactory(colors, effect.Direction));

                view.SetBackground(paint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }

    internal class GradientShaderFactory : ShapeDrawable.ShaderFactory
    {
        internal GradientShaderFactory(int[] colors, GradientDirection direction)
        {
            this.colors = colors;
            this.direction = direction;
        }

        public override Shader Resize(int width, int height)
        {
            LinearGradient gradient = null;

            switch (direction)
            {
                case GradientDirection.ToRight:
                    gradient = new LinearGradient(0, 0, width, 0, colors, null, Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToLeft:
                    gradient = new LinearGradient(width, 0, 0, 0, colors, null, Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToTop:
                    gradient = new LinearGradient(0, height, 0, 0, colors, null, Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToBottom:
                    gradient = new LinearGradient(0, 0, 0, height, colors, null, Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToTopLeft:
                    gradient = new LinearGradient(width, height, 0, 0, colors, null, Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToTopRight:
                    gradient = new LinearGradient(0, height, width, 0, colors, null, Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToBottomLeft:
                    gradient = new LinearGradient(width, 0, 0, height, colors, null, Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToBottomRight:
                    gradient = new LinearGradient(0, 0, width, height, colors, null, Shader.TileMode.Mirror);
                    break;
            }

            return gradient;
        }

        private readonly int[] colors;
        private readonly GradientDirection direction;
    }
}