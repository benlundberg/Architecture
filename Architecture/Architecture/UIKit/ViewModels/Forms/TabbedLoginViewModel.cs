namespace Architecture
{
    public class TabbedLoginViewModel : BaseViewModel
    {
        public LoginViewModel LoginModel { get; set; } = new LoginViewModel();
        public SignUpViewModel SignUpModel { get; set; } = new SignUpViewModel();
    }
}
