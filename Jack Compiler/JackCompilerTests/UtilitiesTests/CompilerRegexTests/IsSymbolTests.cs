using System.Text.RegularExpressions;
using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.CompilerRegexTests;

[TestClass()]
public class IsSymbolTests
{
	[TestMethod()]
	[DataRow("{")]
	[DataRow("}")]
	[DataRow("(")]
	[DataRow(")")]
	[DataRow("[")]
	[DataRow("]")]
	[DataRow(".")]
	[DataRow(",")]
	[DataRow(";")]
	[DataRow("+")]
	[DataRow("-")]
	[DataRow("*")]
	[DataRow("/")]
	[DataRow("&")]
	[DataRow("|")]
	[DataRow("<")]
	[DataRow(">")]
	[DataRow("=")]
	[DataRow("~")]
	public void PassingCorrectSymbol_MatchSuccessIsTrue(string correctSymbol)
	{
		bool expected = true;

		Match match = CompilerRegex.IsSymbol(correctSymbol);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Correct symbol should be treated as such.");
	}

	[TestMethod()]
	[DataRow("{")]
	[DataRow("}")]
	[DataRow("(")]
	[DataRow(")")]
	[DataRow("[")]
	[DataRow("]")]
	[DataRow(".")]
	[DataRow(",")]
	[DataRow(";")]
	[DataRow("+")]
	[DataRow("-")]
	[DataRow("*")]
	[DataRow("/")]
	[DataRow("&")]
	[DataRow("|")]
	[DataRow("<")]
	[DataRow(">")]
	[DataRow("=")]
	[DataRow("~")]
	public void PassingCorrectSymbol_MatchValueEqualsToThePassedSymbol(string correctSymbol)
	{
		string expected = correctSymbol;

		Match match = CompilerRegex.IsSymbol(correctSymbol);
		string actual = match.Value;

		Assert.AreEqual(expected, actual, "Correct symbol should be treated as such.");
	}

	[TestMethod()]
	[DataRow(":")]
	[DataRow("v")]
	[DataRow("and")]
	[DataRow("_")]
	[DataRow("$")]
	[DataRow("")]
	[DataRow("!")]
	[DataRow("@")]
	public void PassingIncorrectSymbol_MatchSuccessIsFalse(string incorrectSymbol)
	{
		bool expected = false;

		Match match = CompilerRegex.IsSymbol(incorrectSymbol);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Incorrect symbol shouldn't have succesful match.");
	}
}