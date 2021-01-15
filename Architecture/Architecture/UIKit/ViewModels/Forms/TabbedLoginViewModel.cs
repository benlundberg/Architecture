namespace Architecture.UIKit
{
    public class TabbedLoginViewModel : BaseViewModel
    {
        public TabbedLoginViewModel()
        {
        }

        public LoginViewModel LoginModel { get; set; } 
        public SignUpViewModel SignUpModel { get; set; }
    }
}
