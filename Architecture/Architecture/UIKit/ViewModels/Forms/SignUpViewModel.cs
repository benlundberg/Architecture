using Architecture.Controls;
using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
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
            if (IsBusy)
            {
                return;
            }
            
            if (!Email.Validate() || !Password.Validate() || !ConfirmPassword.Validate())
            {
                return;
            }

            var loading = new LoadingPopup("Signing you up");

            try
            {
                IsBusy = true;

                await loading.ShowAsync();

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
                ShowAlert(ex.Message, "Exception");
            }
            finally
            {
                IsBusy = false;
                await loading.HideAsync();
            }
        }));

        public ValidatableObject<string> Email { get; set; }
        public ValidatableObject<string> Password { get; set; }
        public ValidatableObject<string> ConfirmPassword { get; set; }
    }
}
