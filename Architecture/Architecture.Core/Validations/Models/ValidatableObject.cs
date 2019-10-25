using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Architecture.Core
{
    public class ValidatableObject<T> : INotifyPropertyChanged
    {
        public ValidatableObject()
        {
            IsValid = true;
        }

        public ValidatableObject(List<IValidationRule<T>> validations)
        {
            this.Validations = validations;

            IsValid = true;
        }

        /// <summary>
        /// Returns true if valid
        /// </summary>
        public bool Validate()
        {
            var errors = Validations?.Where(x => !x.Check(Value)).Select(x => x.ValidationMessage);

            Errors = errors?.ToList();
            IsValid = errors?.Any() != true;

            return this.IsValid;
        }

        public T Value { get; set; }
        public bool IsValid { get; set; }
        public List<IValidationRule<T>> Validations { get; set; }
        public IList<string> Errors { get; set; }
        public string Error => Errors?.FirstOrDefault();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
