using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Demos.UI.List
{
    public class ItemViewModel : BaseViewModel
    {
        public ItemViewModel(ListItemViewModel item, Action<ListItemViewModel, bool> callback)
        {
            this.item = item;
            this.callback = callback;

            Title = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>(Translate("Missing_Title"))
            });

            SubTitle = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>(Translate("Missing_SubTitle"))
            });

            Title.Value = item?.Title;
            SubTitle.Value = item?.SubTitle;
        }

        private ICommand addItemCommand;
        public ICommand AddItemCommand => addItemCommand ?? (addItemCommand = new Command(() =>
        {
            try
            {
                if (!Title.Validate())
                {
                    ShowAlert(Title.Error, Translate("Gen_Item_Title"));
                    return;
                }

                if (!SubTitle.Validate())
                {
                    ShowAlert(SubTitle.Error, Translate("Gen_Item_Title"));
                    return;
                }

                item = new ListItemViewModel()
                {
                    Title = Title.Value,
                    SubTitle = SubTitle.Value
                };

                callback?.Invoke(item, true);

                PopModalCommand?.Execute(null);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
            }
        }));

        private ICommand deleteItemCommand;
        public ICommand DeleteItemCommand => deleteItemCommand ?? (deleteItemCommand = new Command(async (param) =>
        {
            try
            {
                callback?.Invoke(item, false);

                PopModalCommand?.Execute(null);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
            }
        }));

        public ValidatableObject<string> Title { get; set; }
        public ValidatableObject<string> SubTitle { get; set; }

        private ListItemViewModel item;
        private readonly Action<ListItemViewModel, bool> callback;
    }
}
