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
}
