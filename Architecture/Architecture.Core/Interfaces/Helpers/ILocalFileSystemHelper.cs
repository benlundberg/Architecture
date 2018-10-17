using System.Collections.Generic;
using System.IO;

namespace Architecture.Core
{
    public interface ILocalFileSystemHelper
    {
		string LocalStorage { get; }
		string GetLocalPath(params string[] paths);
		string CreateFolder(params string[] paths);
		string CreateFile(params string[] paths);
		bool Delete(params string[] paths);
		byte[] ReadFile(params string[] paths);
		string SaveFile(byte[] data, params string[] paths);
		bool Move(string sourcePath, string destinationPath);
		bool Exists(params string[] paths);
		string WriteText(string text, bool append, params string[] paths);
		string ReadText(params string[] paths);
		void OpenFile(string path);
		FileStream GetFileStream(params string[] paths);
		IEnumerable<string> GetFiles(params string[] paths);
	}
}
