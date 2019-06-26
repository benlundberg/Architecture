using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using Architecture.Core;

namespace Architecture.Droid
{
    public class LocalizeHelper_Droid : ILocalizeHelper
    {
        public string GetCurrentCountry()
        {
            var androidLocale = Java.Util.Locale.Default;
            var localeString = androidLocale.ToString();

            if (localeString.Contains("_"))
            {
                return localeString.Split('_')[1];
            }
            else
            {
                return "US";
            }
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;

            //var netLocale = androidLocale.Language.Replace ("_", "-");
            var netLocale = androidLocale.ToString().Replace("_", "-");

            //var netLanguage = androidLanguage.Replace ("_", "-");
            Console.WriteLine("android:" + androidLocale.ToString());
            Console.WriteLine("net:" + netLocale);

            Console.WriteLine(Thread.CurrentThread.CurrentCulture);
            Console.WriteLine(Thread.CurrentThread.CurrentUICulture);

            return new System.Globalization.CultureInfo(netLocale);
        }

        public void SetLocale()
        {
            var androidLocale = Java.Util.Locale.Default; // user's preferred locale
            var netLocale = androidLocale.ToString().Replace("_", "-");
            var ci = new System.Globalization.CultureInfo(netLocale);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        public IComparer<string> CreateStringComparer(CultureInfo cultureInfo = null)
        {
            StringComparer comparer;
            if (cultureInfo != null)
            {
                comparer = StringComparer.Create(cultureInfo, true);
            }
            else
            {
                comparer = StringComparer.CurrentCultureIgnoreCase;
            }
            return comparer;
        }
    }

}