using System.Text.RegularExpressions;
using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.CompilerRegexTests;

[TestClass()]
public class IsIdentifierTests
{
	[TestMethod()]
	[DataRow("_ValidIdentifier")]
	[DataRow("AnotherValid")]
	[DataRow("validThatContainsNumbers02192")]
	public void PassingCorrectInput_MatchSuccessIsTrue(string correctInput)
	{
		bool expected = true;

		Match match = CompilerRegex.IsIdentifier(correctInput);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Correct identifier should return match that has a truthful 'Success' property.");
	}

	[TestMethod()]
	[DataRow("1111InvalidStartWithNumbers")]
	[DataRow("")]
	[DataRow("()RandomSymbolStart!!!")]
	public void PassingIncorrectInput_MatchSuccessIsFalse(string incorrectInput)
	{
		bool expected = false;

		Match match = CompilerRegex.IsIdentifier(incorrectInput);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Incorrect identifier should return match that has falsy 'Success'.");
	}
}