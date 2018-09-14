using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface INoteManager
    {
        Task<IEnumerable<Note>> GetNotesAsync();
        Task<bool> DeleteNoteAsync(Note note);
        Task<bool> AddNoteAsync(Note note);
        Task<bool> UpdateNoteAsync(Note note);
    }
}
