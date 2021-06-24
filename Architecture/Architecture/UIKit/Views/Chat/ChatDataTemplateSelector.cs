using Architecture.UIKit.ViewModels;
using Xamarin.Forms;

namespace Architecture.UIKit.Views
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MeTemplate { get; set; }
        public DataTemplate OtherTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return (item as ChatItemViewModel)?.IsMe == true ? MeTemplate : OtherTemplate;
        }
    }
}
