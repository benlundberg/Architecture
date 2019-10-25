﻿using System;
using System.Collections.Generic;
using Foundation;
using Architecture.Core;
using System.Threading;
using System.Globalization;

namespace Architecture.iOS
{
    public class LocalizeService_iOS : ILocalizeService
    {
        public string GetCurrentCountry()
        {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var localeString = iosLocaleAuto.ToString();

            if (localeString.Contains("_"))
            {
                return localeString.Split('_')[1];
            }
            else
            {
                return "US";
            }
        }


        public void SetLocale()
        {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var netLocale = iosLocaleAuto.Replace("_", "-");
            System.Globalization.CultureInfo ci;
            try
            {
                ci = new System.Globalization.CultureInfo(netLocale);
            }
            catch
            {
                ci = GetCurrentCultureInfo();
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine("SetLocale: " + ci.Name);
        }

        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            var prefLanguageOnly = "en";
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];

                // HACK: Apple treats portuguese fallbacks in a strange way
                // https://developer.apple.com/library/ios/documentation/MacOSX/Conceptual/BPInternational/LocalizingYourApp/LocalizingYourApp.html
                // "For example, use pt as the language ID for Portuguese as it is used in Brazil and pt-PT as the language ID for Portuguese as it is used in Portugal"
                prefLanguageOnly = pref.Substring(0, 2);
                if (prefLanguageOnly == "pt")
                {
                    if (pref == "pt")
                        pref = "pt-BR"; // get the correct Brazilian language strings from the PCL RESX (note the local iOS folder is still "pt")
                    else
                        pref = "pt-PT"; // Portugal
                }
                netLanguage = pref.Replace("_", "-");
                Console.WriteLine("preferred language:" + netLanguage);
            }

            // this gets called a lot - try/catch can be expensive so consider caching or something
            System.Globalization.CultureInfo ci = null;
            try
            {
                ci = new System.Globalization.CultureInfo(netLanguage);
            }
            catch
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                ci = new System.Globalization.CultureInfo(prefLanguageOnly);
            }

            return ci;
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
