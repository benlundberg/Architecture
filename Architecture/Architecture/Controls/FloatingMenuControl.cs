using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class FloatingMenuControl : StackLayout
    {
        public FloatingMenuControl()
        {
            ButtonItems = new List<Button>();
        }
        
        protected override void OnParentSet()
        {
            base.OnParentSet();

            foreach (var item in ButtonItems)
            {
                // Hide buttons from start
                item.IsVisible = false;
                
                Children.Add(item);
            }

            BaseButton.Clicked += BaseButton_Clicked;

            Children.Add(BaseButton);
        }

        private async void BaseButton_Clicked(object sender, EventArgs e)
        {
            if (Children.All(x => x.IsVisible))
            {
                await HideAsync();
            }
            else
            {
                await ShowAsync();
            }
        }

        private async Task ShowAsync()
        {
            await BaseButton.RotateTo(360, 500);

            foreach (var child in Children.Where(x => x != BaseButton))
            {
                child.IsVisible = !child.IsVisible;
            }
        }

        private async Task HideAsync()
        {
            await BaseButton.RotateTo(-360, 500);

            foreach (var child in Children.Where(x => x != BaseButton))
            {
                child.IsVisible = !child.IsVisible;
            }
        }

        public List<Button> ButtonItems { get; set; }
        public Button BaseButton { get; set; }
    }
}
