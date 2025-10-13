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
	public void PassingCorrectKeyword_ReturnsMatchSuccess(string correctKeyword)
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
	public void PassingCorrectKeyword_ReturnsCorrectMatchValue(string correctKeyword)
	{
		string expected = correctKeyword;

		Match match = CompilerRegex.IsKeyword(correctKeyword);
		string actual = match.Value;

		Assert.AreEqual(expected, actual, $"Correct keyword should return itself as the match.");
	}

	[TestMethod()]
	[DataRow("className")]
	[DataRow("ifTrue")]
	[DataRow("_null")]
	[DataRow("functionCustom")]
	[DataRow("false_01")]
	[DataRow("Constructor")]
	public void PassingIncorrectKeyword_ReturnsFalse(string incorrectKeyword)
	{
		bool expected = false;

		Match match = CompilerRegex.IsKeyword(incorrectKeyword);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Incorrect keyword should return false.");
	}
}