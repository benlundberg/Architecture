using Architecture.Core;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Architecture
{
    public class LoggerViewModel : BaseViewModel
    {
        public override void Appearing()
        {
            base.Appearing();

            LogText = Logger.GetLog();
        }

        private ICommand clearLogCommand;
        public ICommand ClearLogCommand => clearLogCommand ?? (clearLogCommand = new Command(async () =>
        {
            var res = await ShowConfirmAsync($"{Translate("Gen_Clear")} {Translate("Gen_Log")}?", "");

            if (res == false)
            {
                return;
            }

            LogText = string.Empty;
            Logger.ClearLog();
        }));

        private ICommand sendLogCommand;
        public ICommand SendLogCommand => sendLogCommand ?? (sendLogCommand = new Command(async () =>
        {
            try
            {
                await Email.ComposeAsync(Translate("Gen_Log"), LogText);
            }
            catch (System.Exception ex)
            {
                ex.Print();
                ShowAlert(ex.Message, Translate("Gen_Log"));
            }
        }));

        public string LogText { get; private set; }
    }
}
