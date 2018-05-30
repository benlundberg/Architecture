using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class RegisterViewModel : BaseViewModel
    {
        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new Command(async () =>
        {
            await Navigation.PopModalAsync();
        }));
    }
}
