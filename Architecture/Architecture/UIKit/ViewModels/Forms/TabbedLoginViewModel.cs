using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class TabbedLoginViewModel : BaseViewModel
    {
        private ICommand viewOptionChangedCommand;
        public ICommand ViewOptionChangedCommand => viewOptionChangedCommand ?? (viewOptionChangedCommand = new Command((param) =>
        {
            IsLoginVisible = param?.ToString() == "1";
        }));

        public LoginViewModel LoginModel { get; set; } = new LoginViewModel();
        public SignUpViewModel SignUpModel { get; set; } = new SignUpViewModel();

        public bool IsLoginVisible { get; set; } = true;
    }
}
