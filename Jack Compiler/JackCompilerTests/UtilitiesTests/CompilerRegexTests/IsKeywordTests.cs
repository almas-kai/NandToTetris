using System.Text.RegularExpressions;
using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.CompilerRegexTests;

[TestClass()]
public class IsKeywordTests
{
	[TestMethod()]
	[DataRow("class")]
	[DataRow("constructor")]
	[DataRow("function")]
	[DataRow("method")]
	[DataRow("field")]
	[DataRow("static")]
	[DataRow("var")]
	[DataRow("int")]
	[DataRow("char")]
	[DataRow("boolean")]
	[DataRow("void")]
	[DataRow("true")]
	[DataRow("false")]
	[DataRow("null")]
	[DataRow("this")]
	[DataRow("let")]
	[DataRow("do")]
	[DataRow("if")]
	[DataRow("else")]
	[DataRow("while")]
	[DataRow("return")]
	public void PassingCorrectKeyword_MatchSuccessIsTrue(string correctKeyword)
	{
		bool expected = true;

		Match match = CompilerRegex.IsKeyword(correctKeyword);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, $"Correct keyword wasn't recognized as the keyword.");
	}

	[TestMethod()]
	[DataRow("class")]
	[DataRow("constructor")]
	[DataRow("function")]
	[DataRow("method")]
	[DataRow("field")]
	[DataRow("static")]
	[DataRow("var")]
	[DataRow("int")]
	[DataRow("char")]
	[DataRow("boolean")]
	[DataRow("void")]
	[DataRow("true")]
	[DataRow("false")]
	[DataRow("null")]
	[DataRow("this")]
	[DataRow("let")]
	[DataRow("do")]
	[DataRow("if")]
	[DataRow("else")]
	[DataRow("while")]
	[DataRow("return")]
	public void PassingCorrectKeyword_MatchValueEqualsToThePassedKeyword(string correctKeyword)
	{
		string expected = correctKeyword;

		Match match = CompilerRegex.IsKeyword(correctKeyword);
		string actual = match.Value;

		Assert.AreEqual(expected, actual, $"Correct keyword's match should have 'Value' property equal to the matched keyword.");
	}

	[TestMethod()]
	[DataRow("className")]
	[DataRow("ifTrue")]
	[DataRow("_null")]
	[DataRow("functionCustom")]
	[DataRow("false_01")]
	[DataRow("Constructor")]
	public void PassingIncorrectKeyword_MatchSuccessIsFalse(string incorrectKeyword)
	{
		bool expected = false;

		Match match = CompilerRegex.IsKeyword(incorrectKeyword);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Incorrect keyword should have unsuccessful match.");
	}
}