using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IDialogService
    {
        Task<string> ShowSimpleDialog(string title, string[] items, string confirm);
    }
}
