using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class TabbedLoginViewModel : BaseViewModel
    {
        public LoginViewModel LoginModel { get; set; } = new LoginViewModel();
        public SignUpViewModel SignUpModel { get; set; } = new SignUpViewModel();

        public bool IsLoginVisible { get; set; } = true;
    }
}
