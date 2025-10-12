using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.JackFileReaderTests;

[TestClass()]
public class IsCorrectPathTests
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
		JackFileReader jackFileReader = new JackFileReader(correctFilePath);

		bool isCorrectPath = jackFileReader.IsCorrectPath;

		Assert.IsTrue(isCorrectPath, "Correct path must result in truthy IsCorrectPath property.");
	}

	[TestMethod()]
	[DataRow("./Assets/ArrayTestFiles/")]
	[DataRow("./Assets/ExpressionLessSquare/")]
	[DataRow("./Assets/Square/")]
	public void IsCorrectPath_PassingCorrectFolderPath_ReturnsTrue(string correctFolderPath)
	{
		JackFileReader jackFileReader = new JackFileReader(correctFolderPath);

		bool isCorrectPath = jackFileReader.IsCorrectPath;

		Assert.IsTrue(isCorrectPath, "Correct path must result in truthy IsCorrectPath property.");
	}

	[TestMethod()]
	[DataRow("./Assets/ArrayTestFiles/Main.hack")]
	[DataRow("./Assets/ArrayTestFiles/NonExistingFile.jack")]
	[DataRow("./Assets/RandomFolderWithoutJackFiles/")]
	public void IsCorrectPath_PassingIncorrectPath_ReturnsFalse(string incorrectPath)
	{
		JackFileReader jackFileReader = new JackFileReader(incorrectPath);

		bool isCorrectPath = jackFileReader.IsCorrectPath;

		Assert.IsFalse(isCorrectPath, "Incorrect path must result in falsy IsCorrectPath property.");
	}
}