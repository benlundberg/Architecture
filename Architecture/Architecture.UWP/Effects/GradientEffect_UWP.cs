using System.Linq;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("Architecture")]
[assembly: ExportEffect(typeof(Architecture.UWP.GradientEffect_UWP), nameof(Architecture.GradientEffect))]
namespace Architecture.UWP
{
    public class GradientEffect_UWP : PlatformEffect
    {
        protected override void OnAttached()
        {
            var view = Control ?? Container;

            var effect = Element.Effects.OfType<GradientEffect>()?.FirstOrDefault(e => e is GradientEffect);

            if (effect == null)
            {
                return;
            }

            var gradientStops = new GradientStopCollection
            {
                new GradientStop()
                {
                    Color = effect.StartColor.ToWindowsColor(),
                    Offset = 0
                },
                new GradientStop()
                {
                    Color = effect.EndColor.ToWindowsColor(),
                    Offset = .5
                }
            };

            LinearGradientBrush background = null;

            switch (effect.Direction)
            {
                default:
                case GradientDirection.ToRight:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(0, 0.5),
                        EndPoint = new Windows.Foundation.Point(1, 0.5)
                    };
                    break;
                case GradientDirection.ToLeft:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(1, 0.5),
                        EndPoint = new Windows.Foundation.Point(0, 0.5)
                    };
                    break;
                case GradientDirection.ToTop:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(0.5, 1),
                        EndPoint = new Windows.Foundation.Point(0.5, 0)
                    };
                    break;
                case GradientDirection.ToBottom:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(0.5, 0),
                        EndPoint = new Windows.Foundation.Point(0.5, 1)
                    };
                    break;
                case GradientDirection.ToTopLeft:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(1, 1),
                        EndPoint = new Windows.Foundation.Point(0, 0)
                    };
                    break;
                case GradientDirection.ToTopRight:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(0, 1),
                        EndPoint = new Windows.Foundation.Point(1, 0)
                    };
                    break;
                case GradientDirection.ToBottomLeft:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(1, 0),
                        EndPoint = new Windows.Foundation.Point(0, 1)
                    };
                    break;
                case GradientDirection.ToBottomRight:
                    background = new LinearGradientBrush
                    {
                        GradientStops = gradientStops,
                        StartPoint = new Windows.Foundation.Point(0, 0),
                        EndPoint = new Windows.Foundation.Point(1, 1)
                    };
                    break;
            }

            if (view is LayoutRenderer renderer)
            {
                renderer.Background = background;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
