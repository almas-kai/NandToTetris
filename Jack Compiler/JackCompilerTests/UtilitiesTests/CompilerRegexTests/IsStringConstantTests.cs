using System.Text.RegularExpressions;
using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.CompilerRegexTests;

[TestClass()]
public class IsStringConstantTests
{
	[TestMethod()]
	[DataRow("\"Hello World!\"")]
	[DataRow("\"\"")]
	[DataRow("\"  Hello World!!!   \"")]
	public void PassingCorrectInput_MatchSuccessIsTrue(string correctInput)
	{
		bool expected = true;

		Match match = CompilerRegex.IsStringConstant(correctInput);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Correct string constant should return match that has a truthful 'Success' property.");
	}

	[TestMethod()]
	[DataRow("Hello World!")]
	[DataRow("")]
	[DataRow("TEST    S+TEST")]
	public void PassingIncorrectInput_MatchSuccessIsFalse(string incorrectInput)
	{
		bool expected = false;

		Match match = CompilerRegex.IsStringConstant(incorrectInput);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Incorrect string constant should return match that has falsy 'Success'.");
	}
}