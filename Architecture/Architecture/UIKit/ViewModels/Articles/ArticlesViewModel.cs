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
                    ImageSource = "http://lorempixel.com/600/300/nature"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature"
                },
                new ArticleItemViewModel
                {
                    Category = "Nature".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is nature",
                    ImageSource = "http://lorempixel.com/600/300/nature"
                },
            };

            SportArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports"
                },
                new ArticleItemViewModel
                {
                    Category = "Sport".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is sport",
                    ImageSource = "http://lorempixel.com/600/300/sports"
                },
            };

            AnimalArticles = new ObservableCollection<ArticleItemViewModel>
            {
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals"
                },
                new ArticleItemViewModel
                {
                    Category = "Animal".ToUpper(),
                    Year = DateTime.Today.Year.ToString(),
                    Title = "This is animals",
                    ImageSource = "http://lorempixel.com/600/300/animals"
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
