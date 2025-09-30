using System.Collections;

namespace JackCompiler;

internal class JackFileReader : IEnumerable<FileInfo>
{
	private string _Path;

	public JackFileReader(string path)
	{
		_Path = path;
	}

	public bool IsCorrectPath()
	{
		return _IsCorrectFilePath() || _IsCorrectFolderPath();
	}
	private bool _IsCorrectFilePath()
	{
		return _Path.EndsWith(FileExtensions.JACK_EXTENSION) && File.Exists(_Path);
	}
	private bool _IsCorrectFolderPath()
	{
		if (Directory.Exists(_Path))
		{
			return Directory.GetFiles(_Path).Any((string filePath) => filePath.EndsWith(FileExtensions.JACK_EXTENSION));
		}
		return false;
	}
	IEnumerator<FileInfo> IEnumerable<FileInfo>.GetEnumerator()
	{
		throw new NotImplementedException();
	}
	public IEnumerator GetEnumerator()
	{
		return GetEnumerator();
	}
}