using Architecture.Core;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class DialogsViewModel : BaseViewModel
    {
        private ICommand checkDialogCommand;
        public ICommand CheckDialogCommand => checkDialogCommand ?? (checkDialogCommand = new Command(async () =>
        {
            var isChecked = await ComponentContainer.Current.Resolve<IDialogService>().ShowCheckboxDialogAsync("Checkbox dialog", "This is a dialog with a checkbox", "Show again");
        
            IsChecked = isChecked ? Translate("Gen_Yes") : Translate("Gen_No");
        }));

        private ICommand simpleDialogCommand;
        public ICommand SimpleDialogCommand => simpleDialogCommand ?? (simpleDialogCommand = new Command(async () =>
        {
            var option = await ComponentContainer.Current.Resolve<IDialogService>().ShowSimpleDialogAsync("Select", new string[] { "Option 1", "Option 2", "Option 3", "Option 4" });
        }));

        private ICommand alertDialogCommand;
        public ICommand AlertDialogCommand => alertDialogCommand ?? (alertDialogCommand = new Command(() =>
        {
            ShowAlert("This is an alert", "Alert dialog");
        }));

        private ICommand confirmDialogCommand;
        public ICommand ConfirmDialogCommand => confirmDialogCommand ?? (confirmDialogCommand = new Command(async () =>
        {
            var res = await ShowConfirmAsync("Will you confirm this dialog?", "Confirm dialog");

            ConfirmAnswer = res ? Translate("Gen_Yes") : Translate("Gen_No"); 
        }));

        public string IsChecked { get; set; }
        public string ConfirmAnswer { get; set; }
    }
}
