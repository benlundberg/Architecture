using System.Collections.ObjectModel;

namespace Architecture.UIKit.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        public CategoriesViewModel()
        {
            Categories = new ObservableCollection<CategoryItemViewModel>(Services.UIKitService.GetCategories(6));
        }

        public void ItemSelected(string id)
        {
            ShowAlert($"You clicked on {id}", "");
        }

        private CategoryItemViewModel selectedCategory;
        public CategoryItemViewModel SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;

                if (selectedCategory != null)
                {
                    ItemSelected(selectedCategory.Title);

                    SelectedCategory = null;
                }
            }
        }

        public ObservableCollection<CategoryItemViewModel> Categories { get; private set; }
    }
}
