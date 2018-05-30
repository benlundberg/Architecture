using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IZipProvider
    {
        Task<bool> UnzipFileAsync(string fromPath, string toPath, Action<decimal> progress, List<string> exceptionFileTypes = null);
    }
}
