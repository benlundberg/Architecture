using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Globalization;

namespace Architecture.UWP
{
    public class LocalizeHelper_UWP : ILocalizeHelper
    {
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

        public string GetCurrentCountry()
        {
            var geographicRegion = new GeographicRegion();
            return geographicRegion?.CodeTwoLetter ?? "US";
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var locale = GetCurrentCountry();

            Console.WriteLine("uwp:" + locale);

            return new CultureInfo(locale);
        }

        public void SetLocale()
        {
            ApplicationLanguages.PrimaryLanguageOverride = GetCurrentCountry();
        }
    }
}
