using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Architecture
{
    public class ViewContainer
    {
        static readonly Lazy<ViewContainer> implementation = new Lazy<ViewContainer>(() => CreateContainer(), isThreadSafe: true);

        public static ViewContainer Current
        {
            get
            {
                var ret = implementation.Value;

                if (ret == null)
                {
                    throw new NotImplementedException();
                }

                return ret;
            }
        }

        private static ViewContainer CreateContainer()
        {
            return new ViewContainer
            {
                map = new Dictionary<Type, Type>()
            };
        }

        public void Register<TViewModel, TView>()
            where TViewModel : BaseViewModel
            where TView : Page, new()
        {
            if (map == null)
            {
                throw new Exception("Error! ViewFactory is not initialized.");
            }

            map[typeof(TViewModel)] = typeof(TView);
        }

        public Page CreatePage<TViewModel>() where TViewModel : BaseViewModel, new()
        {
            return CreatePage(Activator.CreateInstance<TViewModel>());
        }

        public Page CreatePage<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel
        {
            if (map == null)
            {
                throw new Exception("Error! ViewFactory is not initialized.");
            }

            var viewType = map[typeof(TViewModel)];
            var page = (Page)Activator.CreateInstance(viewType);

            viewModel.OnInitialize();
            viewModel.OnPageCreated(page);

            void Appearing(object sender, EventArgs eventArgs)
            {
                viewModel.Appearing();
                page.Appearing -= Appearing;
            }

            void Disappearing(object sender, EventArgs eventArgs)
            {
                viewModel.Disappearing();
                page.Disappearing -= Disappearing;
            }

            page.Appearing += Appearing;
            page.Disappearing += Disappearing;

            viewModel.Navigation = page.Navigation;
            page.BindingContext = viewModel;

            // Track page in analytics
            Analytics.TrackEvent("Page", new Dictionary<string, string>
            {
                { "Page", page.GetType().ToString() }
            });

            return page;
        }

        private IDictionary<Type, Type> map;
    }
}
