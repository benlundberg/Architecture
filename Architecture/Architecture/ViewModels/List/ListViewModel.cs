using Architecture.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel()
        {
        }

        public override void Appearing()
        {
        }

        private void NoteSelected(object selectedItem)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {

            });
        }

        private object selectedNote;
        public object SelectedNote
        {
            get { return selectedNote; }
            set { selectedNote = value; NoteSelected(selectedNote); }
        }

        public ObservableCollection<object> Items { get; private set; } = new ObservableCollection<object>();
    }
}
