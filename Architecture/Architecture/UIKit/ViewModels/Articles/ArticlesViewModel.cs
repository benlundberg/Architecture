using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
{
    public class ArticlesViewModel : BaseViewModel
    {
        public ArticlesViewModel()
        {
            if (Device.Idiom == TargetIdiom.Desktop)
            {
                Menu = new ObservableCollection<MenuViewModel>
                {
                    new MenuViewModel { Id = 1, Title = "Browser" },
                    new MenuViewModel { Id = 2, Title = "List" },
                };

                SelectedMenuItem = Menu.First();
            }

            LoadData();
        }

        private void LoadData()
        {
            Articles1 = new ObservableCollection<ArticleItemViewModel>();

            for (int i = 0; i < 6; i++)
            {
                Articles1.Add(new ArticleItemViewModel
                {
                    Category = "Nature",
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = ImageService.GetRandomImage()
                });
            }

            Articles2 = new ObservableCollection<ArticleItemViewModel>();
            Articles3 = new ObservableCollection<ArticleItemViewModel>();

            for (int i = 0; i < 12; i++)
            {
                Articles2.Add(new ArticleItemViewModel
                {
                    Category = "Travels",
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is travels",
                    ImageSource = ImageService.GetRandomImage()
                });

                Articles3.Add(new ArticleItemViewModel
                {
                    Category = "Food",
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is foods",
                    ImageSource = ImageService.GetRandomImage()
                });
            }
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

        public ObservableCollection<ArticleItemViewModel> Articles1 { get; private set; }
        public ObservableCollection<ArticleItemViewModel> Articles2 { get; private set; }
        public ObservableCollection<ArticleItemViewModel> Articles3 { get; private set; }

        #region Desktop

        public bool IsList => SelectedMenuItem?.Id != 1;
        public MenuViewModel SelectedMenuItem { get; set; }
        public ObservableCollection<MenuViewModel> Menu { get; set; }

        #endregion
    }

    public class ArticleItemViewModel : BaseItemViewModel
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
        public string ImageSource { get; set; }
    }
}
