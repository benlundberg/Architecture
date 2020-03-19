using Architecture.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel()
        {
            LoadDashboardMenuItems();
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

        private void LoadDashboardMenuItems()
        {
            DashboardMenuItems = new ObservableCollection<DashboardItem>
            {
                new DashboardItem
                {
                    Tag = 1,
                    Title = "Social",
                    ImageBackground = "http://clarityapplication.com/dev/images/9.jpg",
                },
                new DashboardItem
                {
                    Tag = 2,
                    Background = App.Current.PrimaryColor(),
                    Title = "Contacts",
                    NotificationText = "2",
                    IconSource = "\uf0c0"
                },
                new DashboardItem
                {
                    Tag = 3,
                    Background = App.Current.DarkPrimaryColor(),
                    Title = "Photos",
                    IconSource = "\uf030"
                },
                new DashboardItem
                {
                    Tag = 4,
                    Background = App.Current.PrimaryColor(),
                    Title = "World news",
                    IconSource = "\uf7a2",
                    NotificationText = "8"
                },
                new DashboardItem
                {
                    Tag = 5,
                    Background = App.Current.PrimaryColor(),
                    Title = "Calendar",
                    IconSource = "\uf073",
                },
                new DashboardItem
                {
                    Tag = 6,
                    ImageBackground = "http://clarityapplication.com/dev/images/12.jpg",
                    Title = "Food",
                    InfoText = new List<string>
                    {
                        "Joan - ''The food was pretty good and the service was amazing''",
                        "John - ''Who has ketchup on pizza?!''"
                    },
                },
                new DashboardItem
                {
                    Tag = 7,
                    ImageBackground = "http://clarityapplication.com/dev/images/10.jpg",
                    Title = "Travels",
                    InfoText = new List<string>
                    {
                        "Joan - ''San Francisco was so awesome!''",
                    },
                },
                new DashboardItem
                {
                    Tag = 8,
                    Background = App.Current.PrimaryColor(),
                    IconSource = "\uf11b",
                    Title = "Games",
                    NotificationText = "2"
                }
            };
        }

        private void LoadData()
        {
            DashboardItems = new ObservableCollection<DashboardItemViewModel>
            {
                new DashboardItemViewModel
                {
                    Title = "City",
                    Subtitle = "3 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#00363a")
                },
                new DashboardItemViewModel
                {
                    Title = "Animals",
                    Subtitle = "6 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#006064")
                },
                new DashboardItemViewModel
                {
                    Title = "Food",
                    Subtitle = "3 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#428e92")
                },
                new DashboardItemViewModel
                {
                    Title = "Sports",
                    Subtitle = "1 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#005005")
                },
                new DashboardItemViewModel
                {
                    Title = "Business",
                    Subtitle = "7 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#2e7d32")
                },
                new DashboardItemViewModel
                {
                    Title = "Fashion",
                    Subtitle = "3 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#60ad5e")
                },
                new DashboardItemViewModel
                {
                    Title = "Technics",
                    Subtitle = "5 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#6c6f00")
                },
                new DashboardItemViewModel
                {
                    Title = "Transport",
                    Subtitle = "2 news",
                    ImageSource = ImageService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#9e9d24")
                },
                new DashboardItemViewModel
                {
                    Title = "Nature",
                    Subtitle = "1 news",
                    ImageSource = ImageService.GetRandomImage(),
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

        public ObservableCollection<DashboardItem> DashboardMenuItems { get; set; }
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
