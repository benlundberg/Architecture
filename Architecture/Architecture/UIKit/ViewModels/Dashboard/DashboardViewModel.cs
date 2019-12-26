using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel()
        {
            LoadData();
        }

        private ICommand itemClickedCommand;
        public ICommand ItemClickedCommand => itemClickedCommand ?? (itemClickedCommand = new Command((param) =>
        {
            if (!(param is DashboardItemViewModel item))
            {
                return;
            }

            ItemSelected(item.Title);
        }));

        private void LoadData()
        {
            DashboardItems = new ObservableCollection<DashboardItemViewModel>
            {
                new DashboardItemViewModel
                {
                    Title = "City",
                    Subtitle = "3 news",
                    ImageSource = "http://lorempixel.com/200/400/city/1",
                    BackgroundColor = Color.FromHex("#00363a")
                },
                new DashboardItemViewModel
                {
                    Title = "Animals",
                    Subtitle = "6 news",
                    ImageSource = "http://lorempixel.com/200/400/animals/1",
                    BackgroundColor = Color.FromHex("#006064")
                },
                new DashboardItemViewModel
                {
                    Title = "Food",
                    Subtitle = "3 news",
                    ImageSource = "http://lorempixel.com/200/400/food/1",
                    BackgroundColor = Color.FromHex("#428e92")
                },
                new DashboardItemViewModel
                {
                    Title = "Sports",
                    Subtitle = "1 news",
                    ImageSource = "http://lorempixel.com/200/400/sports/1",
                    BackgroundColor = Color.FromHex("#005005")
                },
                new DashboardItemViewModel
                {
                    Title = "Business",
                    Subtitle = "7 news",
                    ImageSource = "http://lorempixel.com/200/400/business/2",
                    BackgroundColor = Color.FromHex("#2e7d32")
                },
                new DashboardItemViewModel
                {
                    Title = "Fashion",
                    Subtitle = "3 news",
                    ImageSource = "http://lorempixel.com/200/400/fashion/1",
                    BackgroundColor = Color.FromHex("#60ad5e")
                },
                new DashboardItemViewModel
                {
                    Title = "Technics",
                    Subtitle = "5 news",
                    ImageSource = "http://lorempixel.com/200/400/technics/1",
                    BackgroundColor = Color.FromHex("#6c6f00")
                },
                new DashboardItemViewModel
                {
                    Title = "Transport",
                    Subtitle = "2 news",
                    ImageSource = "http://lorempixel.com/200/400/transport/1",
                    BackgroundColor = Color.FromHex("#9e9d24")
                },
                new DashboardItemViewModel
                {
                    Title = "Nature",
                    Subtitle = "1 news",
                    ImageSource = "http://lorempixel.com/200/400/nature/1",
                    BackgroundColor = Color.FromHex("#d2ce56")
                }
            };
        }
        
        private void ItemSelected(string title)
        {
            ShowAlert($"You clicked {title}", "Clicked");
        }

        private DashboardItemViewModel selectedItem;
        public DashboardItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                if (selectedItem != null)
                {
                    ItemSelected(selectedItem.Title);

                    SelectedItem = null;
                }
            }
        }

        public ObservableCollection<DashboardItemViewModel> DashboardItems { get; private set; }
    }

    public class DashboardItemViewModel : BaseItemViewModel
    {
        public string ImageSource { get; set; }
        public string IconSource { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public Color BackgroundColor { get; set; }
    }
}
