using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture
{
    public static class LayoutExtension
    {
        public static async Task SlideUpAsync(this Layout layout, double height, uint length, Easing easing)
        {
            layout.HeightRequest = height;
            layout.TranslationY = height;

            await layout.TranslateTo(0, 0, length, easing);
        }

        public static async Task SlideDownAsync(this Layout layout, uint length, Easing easing)
        {
            await layout.TranslateTo(0, layout.Height, length, easing);
        }
    }
}
