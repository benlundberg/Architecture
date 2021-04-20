using Architecture.Controls;
using Architecture.UIKit.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
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
            DashboardMenuItems = new ObservableCollection<TileItem>
            {
                new TileItem
                {
                    Tag = 1,
                    Title = "Social",
                    ImageBackground = "https://architectureappimages.blob.core.windows.net/imagecontainer/9.jpg",
                },
                new TileItem
                {
                    Tag = 2,
                    Background = App.Current.PrimaryColor(),
                    Title = "Contacts",
                    NotificationText = "2",
                    IconSource = "\uf0c0"
                },
                new TileItem
                {
                    Tag = 3,
                    Background = App.Current.DarkPrimaryColor(),
                    Title = "Photos",
                    IconSource = "\uf030"
                },
                new TileItem
                {
                    Tag = 4,
                    Background = App.Current.PrimaryColor(),
                    Title = "World news",
                    IconSource = "\uf7a2",
                    NotificationText = "8"
                },
                new TileItem
                {
                    Tag = 5,
                    Background = App.Current.PrimaryColor(),
                    Title = "Calendar",
                    IconSource = "\uf073",
                },
                new TileItem
                {
                    Tag = 6,
                    ImageBackground = "https://architectureappimages.blob.core.windows.net/imagecontainer/12.jpg",
                    Title = "Food",
                    InfoText = new List<string>
                    {
                        "Joan - ''The food was pretty good and the service was amazing''",
                        "John - ''Who has ketchup on pizza?!''"
                    },
                },
                new TileItem
                {
                    Tag = 7,
                    ImageBackground = "https://architectureappimages.blob.core.windows.net/imagecontainer/10.jpg",
                    Title = "Travels",
                    InfoText = new List<string>
                    {
                        "Joan - ''San Francisco was so awesome!''",
                    },
                },
                new TileItem
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
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#00363a")
                },
                new DashboardItemViewModel
                {
                    Title = "Animals",
                    Subtitle = "6 news",
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#006064")
                },
                new DashboardItemViewModel
                {
                    Title = "Food",
                    Subtitle = "3 news",
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#428e92")
                },
                new DashboardItemViewModel
                {
                    Title = "Sports",
                    Subtitle = "1 news",
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#005005")
                },
                new DashboardItemViewModel
                {
                    Title = "Business",
                    Subtitle = "7 news",
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#2e7d32")
                },
                new DashboardItemViewModel
                {
                    Title = "Fashion",
                    Subtitle = "3 news",
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#60ad5e")
                },
                new DashboardItemViewModel
                {
                    Title = "Technics",
                    Subtitle = "5 news",
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#6c6f00")
                },
                new DashboardItemViewModel
                {
                    Title = "Transport",
                    Subtitle = "2 news",
                    ImageSource = UIKitService.GetRandomImage(),
                    BackgroundColor = Color.FromHex("#9e9d24")
                },
                new DashboardItemViewModel
                {
                    Title = "Nature",
                    Subtitle = "1 news",
                    ImageSource = UIKitService.GetRandomImage(),
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

        public ObservableCollection<TileItem> DashboardMenuItems { get; set; }
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
