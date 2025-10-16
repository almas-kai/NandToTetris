using System.Collections;
using JackCompiler.Utilities;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompilerTests.UtilitiesTests.JackFileReaderTests;

[TestClass()]
public class IEnumerableImplementationTests
{
	[TestMethod()]
	public void JackFileReader_ShouldImplement_IEnumerableOfFileInfo()
	{
		Type jackFileReaderType = typeof(JackFileReader);

		bool implementsGeneric = jackFileReaderType
			.GetInterfaces()
			.Any((Type implementedInterface) =>
			{
				return implementedInterface.IsGenericType &&
					implementedInterface.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
					implementedInterface.GetGenericArguments()[0] == typeof(FileInfo);
			});

		Assert.IsTrue(implementsGeneric, $"{jackFileReaderType.FullName} must implement IEnumerable<T>.");
	}

	[TestMethod()]
	public void JackFileReader_ShouldImplement_NonGenericIEnumerable()
	{
		Type jackFileReaderType = typeof(JackFileReader);

		bool implementedNonGeneric = typeof(IEnumerable).IsAssignableFrom(jackFileReaderType);

		Assert.IsTrue(implementedNonGeneric, $"{jackFileReaderType.FullName} must implement IEnumerable.");
	}

	[TestMethod()]
	[DataRow("./Assets/Square/")]
	[DataRow("./Assets/ExpressionLessSquare/Square.jack")]
	[DataRow("./Assets/ExpressionLessSquare/SquareGame.jack")]
	public void JackFileReader_PassingCorrectPath_ShouldBeEnumeratedWithCorrectPaths(string correctPath)
	{
		List<string> jackFilePaths = new List<string>();
		if (correctPath.EndsWith(FileExtensions.JACK_EXTENSION))
		{
			jackFilePaths.Add(Path.GetFileName(correctPath));
		}
		else
		{
			jackFilePaths.AddRange(
				Directory.GetFiles(correctPath, $"*{FileExtensions.JACK_EXTENSION}")
					.Select((string jackFilePath) => Path.GetFileName(jackFilePath))
			);
		}
		JackFileReader jackFileReader = new JackFileReader(correctPath);
		List<string> actualFilePaths = jackFileReader
			.Select((FileInfo jackFile) => jackFile.Name)
			.ToList();

		CollectionAssert.AreEqual(
			jackFilePaths,
			actualFilePaths,
			"Enumeration did't work as expected."
		);
	}
}