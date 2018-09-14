using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class NoteManager : INoteManager
    {
        // Constructor
        public NoteManager(IDatabaseRepository databaseRepository)
        {
            this.databaseRepository = databaseRepository;
        }

        public Task<bool> AddNoteAsync(Note note)
        {
            return databaseRepository.InsertAsync(note);
        }

        public Task<bool> DeleteNoteAsync(Note note)
        {
            return databaseRepository.DeleteAsync(note);
        }

        public Task<IEnumerable<Note>> GetNotesAsync()
        {
            return databaseRepository.GetAsync<Note>();
        }

        public Task<bool> UpdateNoteAsync(Note note)
        {
            return databaseRepository.UpdateAsync(note);
        }

        private IDatabaseRepository databaseRepository;
    }
}
