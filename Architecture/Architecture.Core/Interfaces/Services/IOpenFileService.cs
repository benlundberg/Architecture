namespace Architecture.Core
{
    public interface IOpenFileService
    {
        void Open(string title, params string[] paths);
    }
}
