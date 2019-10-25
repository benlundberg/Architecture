namespace Architecture.Core
{
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
}
