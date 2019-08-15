using Architecture.Controls;
using Architecture.UWP;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(GradientView), typeof(GradientView_UWP))]

namespace Architecture.UWP
{
    public class GradientView_UWP : VisualElementRenderer<StackLayout, Panel>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                if (e.NewElement is GradientView view)
                {
                    var gradientStops = new GradientStopCollection
                    {
                        new GradientStop()
                        {
                            Color = view.StartColor.ToWindowsColor(),
                            Offset = 0
                        },
                        new GradientStop()
                        {
                            Color = view.EndColor.ToWindowsColor(),
                            Offset = .5
                        }
                    };

                    switch (view.Direction)
                    {
                        default:
                        case GradientDirection.ToRight:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(0, 0.5),
                                EndPoint = new Windows.Foundation.Point(1, 0.5)
                            };
                            break;
                        case GradientDirection.ToLeft:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(1, 0.5),
                                EndPoint = new Windows.Foundation.Point(0, 0.5)
                            };
                            break;
                        case GradientDirection.ToTop:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(0.5, 1),
                                EndPoint = new Windows.Foundation.Point(0.5, 0)
                            };
                            break;
                        case GradientDirection.ToBottom:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(0.5, 0),
                                EndPoint = new Windows.Foundation.Point(0.5, 1)
                            };
                            break;
                        case GradientDirection.ToTopLeft:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(1, 1),
                                EndPoint = new Windows.Foundation.Point(0, 0)
                            };
                            break;
                        case GradientDirection.ToTopRight:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(0, 1),
                                EndPoint = new Windows.Foundation.Point(1, 0)
                            };
                            break;
                        case GradientDirection.ToBottomLeft:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(1, 0),
                                EndPoint = new Windows.Foundation.Point(0, 1)
                            };
                            break;
                        case GradientDirection.ToBottomRight:
                            Background = new LinearGradientBrush
                            {
                                GradientStops = gradientStops,
                                StartPoint = new Windows.Foundation.Point(0, 0),
                                EndPoint = new Windows.Foundation.Point(1, 1)
                            };
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
