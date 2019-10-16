using System.Text.RegularExpressions;

namespace Architecture.Core
{
    /// <summary>
    /// Rule class to check if not null or empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public IsNotNullOrEmptyRule(string validationMessage)
        {
            this.ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(value as string);
        }
    }

    /// <summary>
    /// Rule class to check if input is the required length
    /// </summary>
    public class IsMinimumLengthRule<T> : IValidationRule<T>
    {
        public IsMinimumLengthRule(int lengthRequired, string validationMessage)
        {
            this.lengthRequired = lengthRequired;
            this.ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            return value?.ToString()?.Trim()?.Length >= lengthRequired;
        }

        private readonly int lengthRequired;
    }

    /// <summary>
    /// Rule class to compare if ToString() is same as object?.ToString()
    /// </summary>
    public class IsEqualToRule<T> : IValidationRule<T>
    {
        public IsEqualToRule(ValidatableObject<T> equalToObject, string validationMessage)
        {
            this.equalToObject = equalToObject;
            this.ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            return value?.ToString() == equalToObject.Value?.ToString();
        }

        private readonly ValidatableObject<T> equalToObject;
    }

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

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            try
            {
                //return new System.Net.Mail.MailAddress(value.ToString()).Address == value.ToString();

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
