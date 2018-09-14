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
            try
            {
                var folderPath = localFileSystemHelper.CreateFolder("ImportantNotes");

                var filePath = localFileSystemHelper.CreateFile("ImportantNotes", "SecretNotes");

                var exists = localFileSystemHelper.Exists(filePath);

                exists = localFileSystemHelper.Exists("ImportantNotes", "SecretNotes");

                var res = localFileSystemHelper.Delete("ImportantNotes", "SecretNotes");

                exists = localFileSystemHelper.Exists("ImportantNotes", "SecretNotes");

                folderPath = localFileSystemHelper.CreateFolder("Notes");

                var path = localFileSystemHelper.CreateFile("Notes", "Note1");

                var source = localFileSystemHelper.GetLocalPath("Notes", "Note1");
                var destination = localFileSystemHelper.GetLocalPath("Notes", "Note2");

                res = localFileSystemHelper.Move(source, destination);

                exists = localFileSystemHelper.Exists("Notes", "Note1");
                exists = localFileSystemHelper.Exists("Notes", "Note2");
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public override void Appearing()
        {
            LoadNotes();
        }

        public void LoadNotes()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                ShowLoading();

                var noteList = await GetNoteManager.GetNotesAsync();

                Notes = noteList != null ? new ObservableCollection<Note>(noteList) : new ObservableCollection<Note>();

                HideLoading();
            });
        }

        private void NoteSelected(Note selectedNote)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {

            });
        }

        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set { selectedNote = value; NoteSelected(selectedNote); }
        }

        public ObservableCollection<Note> Notes { get; private set; } = new ObservableCollection<Note>();

        INoteManager GetNoteManager { get; } = ComponentContainer.Current.Resolve<INoteManager>();
        ILocalFileSystemHelper localFileSystemHelper { get; } = ComponentContainer.Current.Resolve<ILocalFileSystemHelper>();
    }
}
