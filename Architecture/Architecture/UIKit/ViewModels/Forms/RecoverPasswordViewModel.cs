using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
	public class RecoverPasswordViewModel : BaseViewModel
	{
		public RecoverPasswordViewModel()
		{
            Email = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>(Translate("Missing_Mail")),
                new IsValidEmailRule<string>(Translate("Invalid_Email"))
            });
        }

        private ICommand sendCommand;
        public ICommand SendCommand => sendCommand ?? (sendCommand = new Command(async () =>
        {
            if (!Email.Validate())
            {
                ShowAlert(Email.Error, Translate("Forgot_Password"));
                return;
            }

            IsBusy = true;

            await Task.Delay(TimeSpan.FromSeconds(1));

            IsBusy = false;

            PopModalCommand.Execute(null);
        }));

        public ValidatableObject<string> Email { get; set; }
    }
}
