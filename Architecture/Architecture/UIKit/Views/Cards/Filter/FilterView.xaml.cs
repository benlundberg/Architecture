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
    public partial class FilterView : PopupPage
    {
        public FilterView()
        {
            InitializeComponent();
        }

        public async Task<FilterOptionsViewModel> ShowAsync(FilterOptionsViewModel filterOptionsViewModel)
        {
            this.FilterOptions = filterOptionsViewModel;

            taskCompletionSource = new TaskCompletionSource<FilterOptionsViewModel>();

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
            taskCompletionSource.SetResult(FilterOptions);
        }));

        private ICommand resetCommand;
        public ICommand ResetCommand => resetCommand ?? (resetCommand = new Command(() =>
        {
            FilterOptions.OnlyShowFavorites = false;
            FilterOptions.RangeUpperValue = 5;
            FilterOptions.RangeLowerValue = 1;
        }));

        public FilterOptionsViewModel FilterOptions { get; set; }

        private TaskCompletionSource<FilterOptionsViewModel> taskCompletionSource;
    }

    public class FilterOptionsViewModel : BaseItemViewModel
    {
        public bool OnlyShowFavorites { get; set; } = false;
        public double RangeUpperValue { get; set; } = 5;
        public double RangeLowerValue { get; set; } = 1;
    }
}