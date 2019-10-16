using Xamarin.Forms;

namespace Architecture
{
    /// <summary>
    /// Extensions to get quick info from application-xaml
    /// </summary>
    public static class ApplicationExtension
    {
        public static Color PrimaryColor(this Application application)
        {
            if (application.Resources["PrimaryColor"] is Color primaryColor)
            {
                return primaryColor;
            }

            return Color.Default;
        }

        public static Color DarkPrimaryColor(this Application application)
        {
            if (application.Resources["DarkPrimaryColor"] is Color darkPrimaryColor)
            {
                return darkPrimaryColor;
            }

            return Color.Default;
        }

        public static Color AccentColor(this Application application)
        {
            if (application.Resources["Accent"] is Color accentColor)
            {
                return accentColor;
            }

            return Color.Default;
        }

        public static Color DarkAccentColor(this Application application)
        {
            if (application.Resources["DarkAccent"] is Color darkAccentColor)
            {
                return darkAccentColor;
            }

            return Color.Default;
        }

        public static string FontAwesomeSolid(this Application application)
        {
            return (OnPlatform<string>)application.Resources["FontAwesomeSolid"];
        }

        public static string FontAwesomeRegular(this Application application)
        {
            return (OnPlatform<string>)application.Resources["FontAwesomeRegular"];
        }
    }
}
