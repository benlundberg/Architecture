using Architecture.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public static class FontTypeService
    {
        private static SKTypeface fontFamily;
        public static SKTypeface GetFontFamily()
        {
            if (fontFamily != null)
            {
                return fontFamily;
            }

            string path = string.Empty;

            if (Device.RuntimePlatform == Device.Android)
            {
                path = "Montserrat-Regular.ttf";
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                path = "Montserrat-Regular";
            }

            if (string.IsNullOrEmpty(path))
            {
                return SKTypeface.Default;
            }

            var stream = ComponentContainer.Current.Resolve<ILocalFileSystemService>().GetStreamFromAssets(path);

            fontFamily = SKTypeface.FromStream(stream);

            return fontFamily;
        }
    }
}
