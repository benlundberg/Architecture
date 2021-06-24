using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.UIKit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortView : PopupPage
    {
        public SortView()
        {
            InitializeComponent();
        }

        public async Task<SortOptionsViewModel> ShowAsync(SortOptionsViewModel sortOptionsViewModel)
        {
            this.SortOptions = sortOptionsViewModel;

            taskCompletionSource = new TaskCompletionSource<SortOptionsViewModel>();

            await PopupNavigation.Instance.PushAsync(this);

            await taskCompletionSource.Task;

            if (PopupNavigation.Instance.PopupStack.Contains(this))
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }

            return taskCompletionSource.Task.Result;
        }

        private ICommand doneCommand;
        public ICommand DoneCommand => doneCommand ?? (doneCommand = new Command(() =>
        {
            taskCompletionSource.SetResult(SortOptions);
        }));

        public SortOptionsViewModel SortOptions { get; set; }

        private TaskCompletionSource<SortOptionsViewModel> taskCompletionSource;
    }

    public class SortOptionsViewModel : BaseItemViewModel
    {
        public bool Date { get; set; } = true;
        public bool Alphabetic { get; set; }
        public bool Rating { get; set; }
        public bool Favorite { get; set; }
    }
}