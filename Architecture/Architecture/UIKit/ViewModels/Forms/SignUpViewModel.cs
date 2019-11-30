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
                new IsNotNullOrEmptyRule<string>(Translate("Missing_Mail")),
                new IsValidEmailRule<string>(Translate("Invalid_Email"))
            });

            Password = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>(Translate("Missing_Password")),
                new IsMinimumLengthRule<string>(lengthRequired: 6, Translate("Invalid_Password_Length"))
            });

            ConfirmPassword = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsEqualToRule<string>(Password, Translate("Invalid_Confirm_Password"))
            });
        }

        private ICommand registerCommand;
        public ICommand RegisterCommand => registerCommand ?? (registerCommand = new Command(async () =>
        {
            if (!Email.Validate())
            {
                ShowAlert(Email.Error, Translate("Gen_Sign_Up"));
                return;
            }

            if (!Password.Validate())
            {
                ShowAlert(Password.Error, Translate("Gen_Sign_Up"));
                return;
            }

            if (!ConfirmPassword.Validate())
            {
                ShowAlert(ConfirmPassword.Error, Translate("Gen_Sign_Up"));
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
