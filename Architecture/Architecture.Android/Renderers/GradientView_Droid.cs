using System;
using Android.Content;
using Architecture.Controls;
using Architecture.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientView), typeof(GradientView_Droid))]

namespace Architecture.Droid
{
    public class GradientView_Droid : VisualElementRenderer<Grid>
    {
        public GradientView_Droid(Context context) : base(context)
        {
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            global::Android.Graphics.LinearGradient gradient;

            var colors = new int[]
            {
                StartColor.ToAndroid().ToArgb(),
                EndColor.ToAndroid().ToArgb()
            };

            switch (Direction)
            {
                default:
                case GradientDirection.ToRight:
                    gradient = new global::Android.Graphics.LinearGradient(0, 0, Width, 0, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToLeft:
                    gradient = new global::Android.Graphics.LinearGradient(Width, 0, 0, 0, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToTop:
                    gradient = new global::Android.Graphics.LinearGradient(0, Height, 0, 0, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToBottom:
                    gradient = new global::Android.Graphics.LinearGradient(0, 0, 0, Height, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToTopLeft:
                    gradient = new global::Android.Graphics.LinearGradient(Width, Height, 0, 0, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToTopRight:
                    gradient = new global::Android.Graphics.LinearGradient(0, Height, Width, 0, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToBottomLeft:
                    gradient = new global::Android.Graphics.LinearGradient(Width, 0, 0, Height, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientDirection.ToBottomRight:
                    gradient = new global::Android.Graphics.LinearGradient(0, 0, Width, Height, colors, null, global::Android.Graphics.Shader.TileMode.Mirror);
                    break;
            }

            var paint = new global::Android.Graphics.Paint()
            {
                Dither = true,
            };

            paint.SetShader(gradient);
            canvas.DrawPaint(paint);

            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Grid> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                if (e.NewElement is GradientView layout)
                {
                    StartColor = layout.StartColor;
                    EndColor = layout.EndColor;
                    Direction = layout.Direction;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public GradientDirection Direction { get; set; }
    }
}