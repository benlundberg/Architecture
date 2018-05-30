using Architecture.Core;
using Xamarin.Forms;

namespace Architecture
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class BaseViewModel
    {
        public virtual void OnInitialize()
        {
        }

        public virtual void Appearing()
        {
        }

        public virtual void Disappearing()
        {
        }

        protected string Translate(string key)
        {
            return TranslateHelper.Translate(key);
        }

        private ITranslateHelper translateHelper;
        public ITranslateHelper TranslateHelper => translateHelper ?? (translateHelper = ComponentContainer.Current.Resolve<ITranslateHelper>());

        public INavigation Navigation { get; set; }
        public bool IsNavigating { get; set; }
        public bool IsBusy { get; set; }
        public bool IsNotBusy
        {
            get
            {
                return !IsBusy;
            }
        }
    }
}
