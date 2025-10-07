using JackCompiler;

namespace JackCompilerTests;

[TestClass()]
public class JackFileReaderTests
{
	[TestMethod()]
	[DataRow("./Assets/ArrayTestFiles/Main.jack")]
	[DataRow("./Assets/ExpressionLessSquare/Main.jack")]
	[DataRow("./Assets/ExpressionLessSquare/Square.jack")]
	[DataRow("./Assets/ExpressionLessSquare/SquareGame.jack")]
	[DataRow("./Assets/Square/Main.jack")]
	[DataRow("./Assets/Square/Square.jack")]
	[DataRow("./Assets/Square/SquareGame.jack")]
	public void IsCorrectPath_PassingCorrectFilePath_ReturnTrue(string correctFilePath)
	{
		bool expected = true;
		JackFileReader jackFileReader = new JackFileReader(correctFilePath);

		bool actual = jackFileReader.IsCorrectPath;

		Assert.AreEqual(expected, actual);
	}

	[TestMethod()]
	[DataRow("./Assets/ArrayTestFiles/")]
	[DataRow("./Assets/ExpressionLessSquare/")]
	[DataRow("./Assets/Square/")]
	public void IsCorrectPath_PassingCorrectFolderPath_ReturnsTrue(string correctFolderPath)
	{
		bool expected = true;
		JackFileReader jackFileReader = new JackFileReader(correctFolderPath);

		bool actual = jackFileReader.IsCorrectPath;

		Assert.AreEqual(expected, actual);
	}

	[TestMethod()]
	[DataRow("./Assets/ArrayTestFiles/Main.hack")]
	[DataRow("./Assets/ArrayTestFiles/NonExistingFile.jack")]
	[DataRow("./Assets/RandomFolderWithoutJackFiles/")]
	public void IsCorrectPath_PassingIncorrectPath_ReturnsFalse(string incorrectPath)
	{
		bool expected = false;
		JackFileReader jackFileReader = new JackFileReader(incorrectPath);

		bool actual = jackFileReader.IsCorrectPath;

		Assert.AreEqual(expected, actual);
	}
}