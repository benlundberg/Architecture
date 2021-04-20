using System.Text.RegularExpressions;

namespace Architecture.Core
{
    /// <summary>
    /// Rule class to compare if string is a valid email address
    /// </summary>
    public class IsValidEmailRule<T> : IValidationRule<T>
    {
        public IsValidEmailRule(string validationMessage)
        {
            this.ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }
        public bool IsOptional { get; set; }

        public bool Check(T value)
        {
            if (IsOptional && string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            if (value == null)
            {
                return false;
            }

            try
            {
                string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                    + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var regex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

                return regex.IsMatch(value.ToString());
            }
            catch (System.Exception ex)
            {
                ex.Print();
            }

            return false;
        }
    }
}
