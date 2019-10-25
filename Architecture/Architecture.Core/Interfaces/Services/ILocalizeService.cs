using System.Collections.Generic;
using System.Globalization;

namespace Architecture.Core
{
    public interface ILocalizeService
    {
        string GetCurrentCountry();
        CultureInfo GetCurrentCultureInfo();
        void SetLocale();
        IComparer<string> CreateStringComparer(CultureInfo cultureInfo = null);
    }
}
