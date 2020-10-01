using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class SignUpViewModel : BaseViewModel
    {
        public SignUpViewModel()
        {
            Email = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provide an e-mail"),
                new IsValidEmailRule<string>("You need to provide a valid e-mail")
            });

            Password = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provide a password"),
                new IsMinimumLengthRule<string>(lengthRequired: 6, "Password needs to be at least six characters long")
            });

            ConfirmPassword = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsEqualToRule<string>(Password, "Passwords needs to be the same")
            });
        }

        private ICommand registerCommand;
        public ICommand RegisterCommand => registerCommand ?? (registerCommand = new Command(async () =>
        {
            if (!Email.Validate())
            {
                ShowAlert(Email.Error, "Sing up");
                return;
            }

            if (!Password.Validate())
            {
                ShowAlert(Password.Error, "Sing up");
                return;
            }

            if (!ConfirmPassword.Validate())
            {
                ShowAlert(ConfirmPassword.Error, "Sing up");
                return;
            }

            IsBusy = true;

            await Task.Delay(TimeSpan.FromSeconds(1.5));

            IsBusy = false;
        }));

        public ValidatableObject<string> Email { get; set; }
        public ValidatableObject<string> Password { get; set; }
        public ValidatableObject<string> ConfirmPassword { get; set; }
    }
}
