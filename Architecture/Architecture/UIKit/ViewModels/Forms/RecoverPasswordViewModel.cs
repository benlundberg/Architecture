using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
{
	public class RecoverPasswordViewModel : BaseViewModel
	{
		public RecoverPasswordViewModel()
		{
            Email = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provide an e-mail"),
                new IsValidEmailRule<string>("You need to provide a valid e-mail")
            });
        }

        private ICommand sendCommand;
        public ICommand SendCommand => sendCommand ?? (sendCommand = new Command(async () =>
        {
            if (!Email.Validate())
            {
                ShowAlert(Email.Error, "Forgot password");
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
