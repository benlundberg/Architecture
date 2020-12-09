using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
{
    public class ArticlesViewModel : BaseViewModel
    {
        public ArticlesViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            NatureArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = ImageService.GetRandomImage()
                },
            };

            SportArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = ImageService.GetRandomImage()
                },
            };

            AnimalArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = ImageService.GetRandomImage()
                },
            };
        }

        private ICommand itemClickedCommand;
        public ICommand ItemClickedCommand => itemClickedCommand ?? (itemClickedCommand = new Command((param) =>
        {
            if (!(param is ArticleItemViewModel item))
            {
                return;
            }

            ItemSelected(item.Title);
        }));

        private void ItemSelected(string title)
        {
            ShowAlert($"You clicked {title}", "Clicked");
        } 

        private ArticleItemViewModel selectedItem;
        public ArticleItemViewModel SelectedItem
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

        public ObservableCollection<ArticleItemViewModel> NatureArticles { get; private set; }
        public ObservableCollection<ArticleItemViewModel> SportArticles { get; private set; }
        public ObservableCollection<ArticleItemViewModel> AnimalArticles { get; private set; }
    }

    public class ArticleItemViewModel : BaseItemViewModel
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
        public string ImageSource { get; set; }
    }
}
