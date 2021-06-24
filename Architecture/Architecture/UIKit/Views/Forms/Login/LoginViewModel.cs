using Architecture.Controls;
using Architecture.Core;
using Architecture.UIKit.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Username = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provide a username")
            });

            Password = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provied a password")
            });
        }

        private ICommand forgotPasswordCommand;
        public ICommand ForgotPasswordCommand => forgotPasswordCommand ?? (forgotPasswordCommand = new Command(async () =>
        {
            await Navigation.PushModalAsync(new ForgotPasswordPage { BindingContext = new ForgotPasswordViewModel { Navigation = this.Navigation } });
        }));

        private ICommand loginCommand;
        public ICommand LoginCommand => loginCommand ?? (loginCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            if (!Username.Validate() || !Password.Validate())
            {
                return;
            }

            var loading = new LoadingPopup("Signing you in");

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
        }));

        private ICommand signUpCommand;
        public ICommand SignUpCommand => signUpCommand ?? (signUpCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                await Navigation.PushModalAsync(new RegisterPage { BindingContext = new RegisterViewModel { Navigation = this.Navigation } });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
                ShowAlert(ex.Message, "Error");
            }
            finally
            {
                IsBusy = false;
            }
        }));

        public ValidatableObject<string> Username { get; set; }
        public ValidatableObject<string> Password { get; set; }
    }
}
