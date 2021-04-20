using Architecture.UIKit.ViewModels;
using System;
using System.Collections.Generic;

namespace Architecture.UIKit.Services
{
    public static class UIKitService
    {
        public static string GetRandomImage()
        {
            Random random = new Random();

            string baseUrl = "https://architectureappimages.blob.core.windows.net/imagecontainer/";

            int picture = random.Next(1, 27);

            return baseUrl + picture.ToString() + ".jpg";
        }

        public static List<ArticleItemViewModel> GetArticles(int max)
        {
            Random random = new Random();

            var list = new List<ArticleItemViewModel>();

            for (int i = 0; i < max; i++)
            {
                var rand = random.Next(1, 20);

                list.Add(new ArticleItemViewModel
                {
                    Title = "Headline",
                    Text = rand % 2 == 0 ? "Lorem ipsum dolor sit amet" : "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor",
                    ImageSource = GetRandomImage()
                });
            }

            return list;
        }

        public static List<CategoryItemViewModel> GetCategories(int max)
        {
            var list = new List<CategoryItemViewModel>();

            var array = new string[]
            {
                "Nature",
                "Space",
                "Food",
                "News",
                "Sports",
                "Architecture",
                "Medicin",
                "TV",
                "Personal",
                "Construction"
            };

            for (int i = 0; i < max; i++)
            {
                list.Add(new CategoryItemViewModel
                {
                    Title = array[i],
                    ImageSource = GetRandomImage()
                });
            }

            return list;
        }

        public static List<ListItemViewModel> GetListItems(int max)
        {
            Random random = new Random();

            var list = new List<ListItemViewModel>();
            
            for (int i = 0; i < max; i++)
            {
                var rand = random.Next(1, 20);
                var rating = random.Next(1, 5);

                list.Add(new ListItemViewModel
                {
                    Id = i,
                    Title = "Header",
                    SubTitle = rand % 2 == 0 ? "Lorem ipsum dolor sit amet" : "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor",
                    ImageSource = GetRandomImage(),
                    Rating = rating
                });
            }

            return list;
        }
    }
}
