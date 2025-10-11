using System.Collections;

namespace JackCompiler;

internal class JackFileReader : IEnumerable<FileInfo>
{
	private string _Path;
	public bool IsCorrectPath { get; private set; }
	private List<FileInfo> _jackFilePaths = new List<FileInfo>();
	public JackFileReader(string path)
	{
		_Path = path;

		if (IsCorrectFilePath(_Path))
		{
			_jackFilePaths.Add(new FileInfo(_Path));
		}
		else if (_IsCorrectFolderPath())
		{
			_jackFilePaths.AddRange(
				Directory.GetFiles(_Path, $"*{FileExtensions.JACK_EXTENSION}")
					.Select((string jackFilePath) => new FileInfo(jackFilePath))
			);
		}

		IsCorrectPath = _jackFilePaths.Count > 0;
	}
	private bool _IsCorrectFolderPath()
	{
		if (Directory.Exists(_Path))
		{
			return Directory.GetFiles(_Path, $"*{FileExtensions.JACK_EXTENSION}").Any();
		}
		return false;
	}
	public static bool IsCorrectFilePath(string potentialJackFile)
	{
		return potentialJackFile.EndsWith(FileExtensions.JACK_EXTENSION) && File.Exists(potentialJackFile);
	}
	IEnumerator<FileInfo> IEnumerable<FileInfo>.GetEnumerator()
	{
		return _jackFilePaths.GetEnumerator();
	}
	public IEnumerator GetEnumerator()
	{
		return GetEnumerator();
	}
}