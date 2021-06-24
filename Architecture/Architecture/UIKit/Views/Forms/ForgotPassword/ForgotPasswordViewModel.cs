using Architecture.Controls;
using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public ForgotPasswordViewModel()
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
                return;
            }

            var loading = new LoadingPopup("Sending you a restore link");

            try
            {
                IsBusy = true;

                await loading.ShowAsync();

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
                ShowAlert(ex.Message, "Error");
            }
            finally
            {
                IsBusy = false;
                await loading.HideAsync();
            }

            try
            {
                var notification = new NotificationPopup(new NotificationOption
                {
                    MessageTitle = "Check your email",
                    Message = "A restore link have been sent to your email"
                });

                await notification.ShowAsync();
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            PopModalCommand.Execute(null);
        }));

        public ValidatableObject<string> Email { get; set; }
    }
}
