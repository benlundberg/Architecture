using CoreAnimation;
using CoreGraphics;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Architecture")]
[assembly: ExportEffect(typeof(Architecture.iOS.GradientEffect_iOS), nameof(Architecture.GradientEffect))]
namespace Architecture.iOS
{
    public class GradientEffect_iOS : PlatformEffect
    {
        protected override void OnAttached()
        {
            var view = Control ?? Container;

            var effect = Element.Effects.OfType<GradientEffect>()?.FirstOrDefault(e => e is GradientEffect);

            if (effect == null)
            {
                return;
            }

            CGColor[] colors = new CGColor[]
            {
                effect.StartColor.ToCGColor(),
                effect.EndColor.ToCGColor()
            };

            var gradientLayer = new CAGradientLayer();

            switch (effect.Direction)
            {
                default:
                case GradientDirection.ToRight:
                    gradientLayer.StartPoint = new CGPoint(0, 0.5);
                    gradientLayer.EndPoint = new CGPoint(1, 0.5);
                    break;
                case GradientDirection.ToLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 0.5);
                    gradientLayer.EndPoint = new CGPoint(0, 0.5);
                    break;
                case GradientDirection.ToTop:
                    gradientLayer.StartPoint = new CGPoint(0.5, 0);
                    gradientLayer.EndPoint = new CGPoint(0.5, 1);
                    break;
                case GradientDirection.ToBottom:
                    gradientLayer.StartPoint = new CGPoint(0.5, 1);
                    gradientLayer.EndPoint = new CGPoint(0.5, 0);
                    break;
                case GradientDirection.ToTopLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 0);
                    gradientLayer.EndPoint = new CGPoint(0, 1);
                    break;
                case GradientDirection.ToTopRight:
                    gradientLayer.StartPoint = new CGPoint(0, 1);
                    gradientLayer.EndPoint = new CGPoint(1, 0);
                    break;
                case GradientDirection.ToBottomLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 1);
                    gradientLayer.EndPoint = new CGPoint(0, 0);
                    break;
                case GradientDirection.ToBottomRight:
                    gradientLayer.StartPoint = new CGPoint(0, 0);
                    gradientLayer.EndPoint = new CGPoint(1, 1);
                    break;
            }
            
            if (view.Layer.Bounds.IsEmpty)
            {
                view.BackgroundColor = effect.StartColor.ToUIColor();
            }
            else
            {
                view.Layer.MasksToBounds = true;
                
                gradientLayer.Frame = view.Bounds;
                gradientLayer.Colors = colors;

                view.Layer.InsertSublayer(gradientLayer, 0);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}