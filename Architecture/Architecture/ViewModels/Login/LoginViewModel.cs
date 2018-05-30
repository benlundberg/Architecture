using Acr.UserDialogs;
using Architecture.Core;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            loginService = ComponentContainer.Current.Resolve<ILoginService>();
        }

        private ICommand loginCommand;
        public ICommand LoginCommand => loginCommand ?? (loginCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                UserDialogs.Instance.Alert(Translate("Missing_Login_Input"));
                return;
            }

            try
            {
                IsBusy = true;

                UserDialogs.Instance.ShowLoading(Translate("Login_In_Process"));

                var res = await loginService.LoginAsync<object>(Username, Password);

                UserDialogs.Instance.HideLoading();

                App.SetMainPage(isLoggedIn: true);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message, Translate("Login_Fail"));
            }
            finally
            {
                IsBusy = false;
            }
        }));

        private ICommand registerCommand;
        public ICommand RegisterCommand => registerCommand ?? (registerCommand = new Command(async () =>
        {
            await Navigation.PushModalAsync(new NavigationPage(ViewContainer.Current.CreatePage<RegisterViewModel>()));
        }));

        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        private ILoginService loginService;
    }
}
