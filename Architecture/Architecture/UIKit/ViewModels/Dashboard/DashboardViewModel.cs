using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace Architecture
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel()
        {
            DashboardItems = new ObservableCollection<DashboardItemViewModel>
            {
                new DashboardItemViewModel
                {
                    Title = "City",
                    SubTitle = "3 news",
                    ImageSource = "http://lorempixel.com/200/400/city/1",
                    BackgroundColor = Color.FromHex("#00363a")
                },
                new DashboardItemViewModel
                {
                    Title = "Animals",
                    SubTitle = "6 news",
                    ImageSource = "http://lorempixel.com/200/400/animals/1",
                    BackgroundColor = Color.FromHex("#006064")
                },
                new DashboardItemViewModel
                {
                    Title = "Food",
                    SubTitle = "3 news",
                    ImageSource = "http://lorempixel.com/200/400/food/1",
                    BackgroundColor = Color.FromHex("#428e92")
                },
                new DashboardItemViewModel
                {
                    Title = "Sports",
                    SubTitle = "1 news",
                    ImageSource = "http://lorempixel.com/200/400/sports/1",
                    BackgroundColor = Color.FromHex("#005005")
                },
                new DashboardItemViewModel
                {
                    Title = "Business",
                    SubTitle = "7 news",
                    ImageSource = "http://lorempixel.com/200/400/business/2",
                    BackgroundColor = Color.FromHex("#2e7d32")
                },
                new DashboardItemViewModel
                {
                    Title = "Fashion",
                    SubTitle = "3 news",
                    ImageSource = "http://lorempixel.com/200/400/fashion/1",
                    BackgroundColor = Color.FromHex("#60ad5e")
                },
                new DashboardItemViewModel
                {
                    Title = "Technics",
                    SubTitle = "5 news",
                    ImageSource = "http://lorempixel.com/200/400/technics/1",
                    BackgroundColor = Color.FromHex("#6c6f00")
                },
                new DashboardItemViewModel
                {
                    Title = "Transport",
                    SubTitle = "2 news",
                    ImageSource = "http://lorempixel.com/200/400/transport/1",
                    BackgroundColor = Color.FromHex("#9e9d24")
                },
                new DashboardItemViewModel
                {
                    Title = "Nature",
                    SubTitle = "1 news",
                    ImageSource = "http://lorempixel.com/200/400/nature/1",
                    BackgroundColor = Color.FromHex("#d2ce56")
                }
            };
        }

        public ObservableCollection<DashboardItemViewModel> DashboardItems { get; private set; }
    }

    public class DashboardItemViewModel : INotifyPropertyChanged
    {
        public string ImageSource { get; set; }
        public string IconSource { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Color BackgroundColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
