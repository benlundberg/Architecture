using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class SkeletonView : StackLayout
    {
        private void StartAnimation()
        {
            this.Opacity = 1;
            this.Children.Clear();

            for (int i = 0; i < Repeat; i++)
            {
                Children.Add(DataTemplate.CreateContent() as View);
            }

            Task.Run(async () => { await Animate(); });
        }

        public async Task Animate()
        {
            if (!IsLoading)
            {
                this.Opacity = 1;
                return;
            }

            await this.FadeTo(.5, 1000);
            await this.FadeTo(1, 1000);

            await Animate();
        }

        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
            propertyName: "IsLoading",
            returnType: typeof(bool),
            declaringType: typeof(SkeletonView),
            defaultValue: default,
            propertyChanged: IsLoadingChanged);

        private static void IsLoadingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is SkeletonView view))
            {
                return;
            }

            if (newValue == null)
            {
                return;
            }

            if ((bool)newValue)
            {
                view.IsVisible = true;
                view.StartAnimation();
            }
            else
            {
                view.IsVisible = false;
            }
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public DataTemplate DataTemplate { get; set; }
        public int Repeat { get; set; } = 3;
    }
}
