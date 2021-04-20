using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            ExamplePageItems = new ObservableCollection<GroupedExamplePageItem>
            {
                new GroupedExamplePageItem("Articles", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Article Browser",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ArticlesBrowserViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Article Browser Variant",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ArticlesBrowserVariantViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Article List",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ArticlesListViewModel>());
                        })
                    }
                }),
                new GroupedExamplePageItem("Categories", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Category List",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.CategoryListPage { BindingContext = new CategoriesViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Category Grid",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.CategoryGridPage { BindingContext = new CategoriesViewModel { Navigation = this.Navigation } });
                        })
                    }
                }),
                new GroupedExamplePageItem("Forms", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Full Login",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushModalAsync(ViewContainer.Current.CreatePage<LoginViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Sign Up",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushModalAsync(ViewContainer.Current.CreatePage<SignUpViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Tabbed Login",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<TabbedLoginViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Image Login",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushModalAsync(new Views.Phone.ImageLoginPage { BindingContext = new LoginViewModel { Navigation = this.Navigation } });
                        })
                    }
                }),
                new GroupedExamplePageItem("List", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "List",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.Phone.ListPage { BindingContext = new ListViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "List Card",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.Phone.ListCardPage { BindingContext = new ListViewModel { Navigation = this.Navigation } });
                        })
                    }
                }),
                new GroupedExamplePageItem("Details", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Detail",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.Phone.DetailPage { BindingContext = new DetailViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Detail Variant",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.Phone.VariantDetailPage { BindingContext = new DetailViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Second Detail",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.Phone.SecondDetailPage { BindingContext = new DetailViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Third Detail",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.Phone.ThirdDetailPagexaml { BindingContext = new DetailViewModel { Navigation = this.Navigation } });
                        })
                    },
                }),
                new GroupedExamplePageItem("Settings", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Settings",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<SettingsViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Feedback",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<FeedbackViewModel>());      
                        })
                    }
                }),
                new GroupedExamplePageItem("Data", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Data",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<DataViewModel>());
                        })
                    }
                }),
                new GroupedExamplePageItem("Components", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Components",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.ComponentsPage());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Dialogs",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.DialogsPage());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Camera",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new Views.MediaPage());
                        })
                    }
                })
            };
        }

        public ObservableCollection<GroupedExamplePageItem> ExamplePageItems { get; private set; }
    }

    public class ExamplePageItem
    {
        public ICommand Command { get; set; }
        public string Title { get; set; }
    }

    public class GroupedExamplePageItem : ObservableCollection<ExamplePageItem>
    {
        public GroupedExamplePageItem(string title, List<ExamplePageItem> items) : base(items)
        {
            this.Title = title;
        }

        public string Title { get; set; }
    }
}
