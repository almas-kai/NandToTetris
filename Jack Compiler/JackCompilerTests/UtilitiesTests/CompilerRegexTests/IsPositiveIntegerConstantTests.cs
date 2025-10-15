using System.Text.RegularExpressions;
using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.CompilerRegexTests;

[TestClass()]
public class IsPositiveIntegerConstantTests
{
	[TestMethod()]
	[DataRow("0")]
	[DataRow("1")]
	[DataRow("100")]
	[DataRow("100000")]
	[DataRow("15")]
	[DataRow("256")]
	[DataRow("-99")]
	[DataRow("-89.23.222")]
	[DataRow("+232=232")]
	public void PassingPseudoNumber_MatchSuccessIsTrue(string pseudoNumber)
	{
		bool expected = true;

		Match match = CompilerRegex.IsIntegerConstant(pseudoNumber);
		bool actual = match.Success;

		// Note this regex is for extraction the number from the instruction. Validation happens in the tokenizer.
		Assert.AreEqual(expected, actual, "Any number delimitted by word boundary must be valid.");
	}

	[TestMethod()]
	[DataRow("N0")]
	[DataRow("1F")]
	[DataRow("100aa")]
	[DataRow("100000ss")]
	[DataRow("t15s")]
	[DataRow("R256")]
	[DataRow("-RS99")]
	[DataRow("-sss89.Lok23.2s222")]
	[DataRow("+232Pop=s232")]
	public void PassingInvalidData_MatchSuccessIsFalse(string invalidInput)
	{
		bool expected = false;

		Match match = CompilerRegex.IsIntegerConstant(invalidInput);
		bool actual = match.Success;

		Assert.AreEqual(expected, actual, "Invalid data should fail regex match.");
	}
}