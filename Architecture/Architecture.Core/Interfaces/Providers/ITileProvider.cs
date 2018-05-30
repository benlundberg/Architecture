namespace Architecture.Core
{
    public interface ITileProvider
    {
        byte[] GetTile(string path, int x, int y, int zoom, string extension = ".png");
    }
}
