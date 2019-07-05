using Architecture.Core;
using Architecture.Demos.UI.Register;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Demos.UI.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private ICommand loginCommand;
        public ICommand LoginCommand => loginCommand ?? (loginCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            if (string.IsNullOrEmpty(Username))
            {
                ShowAlert(Translate("Missing_Username"), Translate("Gen_Login"));
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                ShowAlert(Translate("Missing_Password"), Translate("Gen_Login"));
                return;
            }

            try
            {
                IsBusy = true;

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                ex.Print();
            }
            finally
            {
                IsBusy = false;
            }
        }));

        private ICommand registerCommand;
        public ICommand RegisterCommand => registerCommand ?? (registerCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                await Navigation.PushAsync(ViewContainer.Current.CreatePage<RegisterViewModel>());
            }
            catch (Exception ex)
            {
                ex.Print();
            }
            finally
            {
                IsBusy = false;
            }
        }));

        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
