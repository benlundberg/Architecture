using Architecture.UIKit.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class UIKitHomeViewModel : BaseViewModel
    {
        public UIKitHomeViewModel()
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
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ArticleBrowserViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Article Browser Variant",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ArticleBrowserVariantViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Article List",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ArticleListViewModel>());
                        })
                    }
                }),
                new GroupedExamplePageItem("Articles", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Login",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushModalAsync(new LoginPage { BindingContext = new LoginViewModel { Navigation = this.Navigation } });
                        }),
                    },
                    new ExamplePageItem
                    {
                        Title = "Image Login",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushModalAsync(new ImageLoginPage { BindingContext = new LoginViewModel { Navigation = this.Navigation } });
                        }),
                    },
                    new ExamplePageItem
                    {
                        Title = "Tabbed Login",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new TabbedLoginPage
                            {
                                BindingContext = new TabbedLoginViewModel
                                {
                                    LoginViewModel = new LoginViewModel
                                    {
                                        Navigation = this.Navigation
                                    },
                                    RegisterViewModel = new RegisterViewModel
                                    {
                                        Navigation = this.Navigation
                                    },
                                    Navigation = this.Navigation
                                }
                            });
                        }),
                    },
                    new ExamplePageItem
                    {
                        Title = "Register",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushModalAsync(new RegisterPage { BindingContext = new RegisterViewModel { Navigation = this.Navigation } });
                        }),
                    }
                }),
                new GroupedExamplePageItem("List", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "List",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ListViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Cards List",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<CardsListViewModel>());
                        })
                    }
                }),
                new GroupedExamplePageItem("Details", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Details",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new DetailsPage { BindingContext = new DetailsViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Details Variant",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new DetailsVariantPage { BindingContext = new DetailsViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Details Second",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new DetailsSecondPage { BindingContext = new DetailsViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Details Third",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new DetailsThirdPage { BindingContext = new DetailsViewModel { Navigation = this.Navigation } });
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Details Information",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(new DetailsInformationPage { BindingContext = new DetailsViewModel { Navigation = this.Navigation } });
                        })
                    }
                }),
                new GroupedExamplePageItem("Messages", new List<ExamplePageItem>
                {
                    new ExamplePageItem
                    {
                        Title = "Messages",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<MessagesViewModel>());
                        })
                    },
                    new ExamplePageItem
                    {
                        Title = "Chat",
                        Command = new Command(async () =>
                        {
                            await Navigation.PushAsync(ViewContainer.Current.CreatePage<ChatViewModel>());
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
