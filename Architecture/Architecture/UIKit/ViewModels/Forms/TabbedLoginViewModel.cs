namespace Architecture.UIKit.ViewModels
{
    public class TabbedLoginViewModel : BaseViewModel
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            LoginModel = new LoginViewModel { Navigation = this.Navigation };
            SignUpModel = new SignUpViewModel { Navigation = this.Navigation };
        }

        public LoginViewModel LoginModel { get; set; }
        public SignUpViewModel SignUpModel { get; set; }
    }
}
