using System;
using System.Diagnostics;

namespace Architecture.Core
{
    public static class ExceptionExtensions
    {
        public static void Print(this Exception ex)
        {
            Debug.WriteLine("Message: " + ex.Message);
            Debug.WriteLine("StackTrace: " + ex.StackTrace);
            Debug.WriteLine("InnerException: " + ex.InnerException?.Message);
        }
    }
}
