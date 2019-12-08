using System;
using System.Collections.ObjectModel;

namespace Architecture
{
    public class ArticlesViewModel : BaseViewModel
    {
        public ArticlesViewModel()
        {
            NatureArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature/1"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature/2"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature/3"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature/4"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature/5"
                },
            };

            SportArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports/1"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports/2"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports/3"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports/4"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports/5"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports/6"
                },
            };

            AnimalArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals/1"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals/2"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals/3"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals/6"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals/7"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals/8"
                },
            };
        }

        public ObservableCollection<ArticleItemViewModel> NatureArticles { get; private set; }
        public ObservableCollection<ArticleItemViewModel> SportArticles { get; private set; }
        public ObservableCollection<ArticleItemViewModel> AnimalArticles { get; private set; }
    }

    public class ArticleItemViewModel
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
        public string ImageSource { get; set; }
    }
}
