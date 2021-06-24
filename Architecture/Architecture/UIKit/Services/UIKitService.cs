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

            string baseUrl = "https://raw.githubusercontent.com/benlundberg/clarityapps.io/gh-pages/images/";

            int picture = random.Next(1, 37);

            return baseUrl + picture.ToString() + ".jpg";
        }

        public static List<ChatItemViewModel> GetChats()
        {
            return new List<ChatItemViewModel>
            {
                new ChatItemViewModel { IsMe = false, Message = "Lorem ipsum dolor sit" },
                new ChatItemViewModel { IsMe = true, Message = "Lorem ipsum dolor sit" },
                new ChatItemViewModel { IsMe = true, Message = "Lorem ipsum" },
                new ChatItemViewModel { IsMe = false, Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new ChatItemViewModel { IsMe = false, Message = "Lorem ipsum dolor sit" },
                new ChatItemViewModel { IsMe = true, Message = "Lorem ipsum dolor sit" },
                new ChatItemViewModel { IsMe = true, Message = "Lorem ipsum" },
                new ChatItemViewModel { IsMe = false, Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new ChatItemViewModel { IsMe = false, Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor", Status = "30 min ago" },
                new ChatItemViewModel { IsMe = true, Message = "Lorem ipsum dolor sit", Status = "Seen" },
            };
        }

        public static List<MessageItemViewModel> GetMessages()
        {
            return new List<MessageItemViewModel>
            {
                new MessageItemViewModel
                {
                    From = "John Doe",
                    Message = "Lorem ipsum dolor sit amet, consectetur",
                    Online = true,
                    ProfilePicture = "https://raw.githubusercontent.com/benlundberg/clarityapps.io/gh-pages/images/5.jpg",
                    Time = "10 min ago",
                    UnseenMessages = 2
                },
                new MessageItemViewModel
                {
                    From = "Joan Doe",
                    Message = "Lorem ipsum dolor sit amet, consectetur",
                    Online = false,
                    ProfilePicture = "https://raw.githubusercontent.com/benlundberg/clarityapps.io/gh-pages/images/9.jpg",
                    Time = "15 min ago",
                    UnseenMessages = 1
                },
                new MessageItemViewModel
                {
                    From = "John Doe",
                    Message = "Lorem ipsum dolor sit amet",
                    Online = true,
                    ProfilePicture = "https://raw.githubusercontent.com/benlundberg/clarityapps.io/gh-pages/images/13.jpg",
                    Time = "2 days ago",
                    UnseenMessages = 0
                },
                new MessageItemViewModel
                {
                    From = "Joan Doe",
                    Message = "Lorem ipsum dolor sit amet, consectetur",
                    Online = true,
                    ProfilePicture = "https://raw.githubusercontent.com/benlundberg/clarityapps.io/gh-pages/images/22.jpg",
                    Time = "2021-05-24",
                    UnseenMessages = 0
                },
            };
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
