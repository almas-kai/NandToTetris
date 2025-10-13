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
}