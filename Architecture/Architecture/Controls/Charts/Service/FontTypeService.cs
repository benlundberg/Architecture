using Architecture.Core;
using SkiaSharp;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public static class FontTypeService
    {
        private static SKTypeface fontFamily;
        public static SKTypeface GetFontFamily(Assembly assembly)
        {
            if (fontFamily != null)
            {
                return fontFamily;
            }

            try
            {
                string resourceID = "Architecture.Resources.Fonts.OpenSans-Regular.ttf";

                using Stream stream = assembly.GetManifestResourceStream(resourceID);

                fontFamily = SKTypeface.FromStream(stream);

                stream.Close();
            }
            catch (Exception ex)
            {
                ex.Print();

                fontFamily = SKTypeface.Default;
            }

            return fontFamily;
        }
    }
}
