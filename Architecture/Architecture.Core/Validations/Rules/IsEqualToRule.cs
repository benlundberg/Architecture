namespace Architecture.Core
{
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
}
