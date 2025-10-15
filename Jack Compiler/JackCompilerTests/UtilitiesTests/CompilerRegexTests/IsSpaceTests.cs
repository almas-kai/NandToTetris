using System.Text.RegularExpressions;
using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.CompilerRegexTests;

[TestClass()]
public class IsSpaceTests
{
	[TestMethod()]
	[DataRow(" ")]
	[DataRow("   ")]
	[DataRow("\n")]
	[DataRow("\r")]
	[DataRow(" \r\n")]
	[DataRow("\t")]
	[DataRow("\t\t\t")]
	public void PassingCorrectInput_MatchSuccessIsTrue(string correctInput)
	{
		bool expected = true;

		Match match = CompilerRegex.IsSpace(correctInput);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Space chracters should be matched correctly.");
	}

	[TestMethod()]
	[DataRow("")]
	[DataRow("Hello")]
	[DataRow(":;")]
	[DataRow("!!!")]
	[DataRow("\'\'")]
	[DataRow("TAB")]
	public void PassingIncorrectInput_MatchSuccessIsFalse(string incorrectInput)
	{
		bool expected = false;

		Match match = CompilerRegex.IsSpace(incorrectInput);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Non space chracters should return match with falsy 'Success' property.");
	}
}