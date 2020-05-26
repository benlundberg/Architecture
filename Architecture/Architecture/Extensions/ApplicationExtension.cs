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

        public static Color GrayColor(this Application application)
        {
            if (application.Resources["Gray"] is Color color)
            {
                return color;
            }

            return Color.Default;
        }

        public static Color LightGrayColor(this Application application)
        {
            if (application.Resources["GrayLight"] is Color color)
            {
                return color;
            }

            return Color.Default;
        }

        public static Color ToolbarTextColor(this Application application)
        {
            if (application.Resources["ToolbarTextColor"] is Color color)
            {
                return color;
            }

            return Color.Default;
        }

        public static string FontAwesomeSolid(this Application application)
        {
            return application.Resources["FontAwesomeSolid"]?.ToString();
        }

        public static string FontAwesomeRegular(this Application application)
        {
            return application.Resources["FontAwesomeRegular"]?.ToString();
        }

        public static T Get<T>(this Application application, string key)
        {
            return (T)application.Resources[key];
        }
    }
}
