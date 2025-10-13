using JackCompiler.Utilities;

namespace JackCompilerTests.UtilitiesTests.CompilerRegexTests;

[TestClass()]
public class RemoveAllComments
{
	[TestMethod()]
	[
		DataRow(
			"""
				// This file is part of www.nand2tetris.org
			""",
			""
		),
		DataRow(
			"""
				// This file is part of www.nand2tetris.org
				// and the book "The Elements of Computing Systems"
				// by Nisan and Schocken, MIT Press.
				// File name: projects/10/ExpressionLessSquare/Main.jack
			""",
			""
		),
		DataRow(
			"""
				static boolean test;    // Added for testing -- there is no static keyword
			""",
			"static boolean test;"
		),
		DataRow(
			"""
				function void main() { // Comment!
			""",
			"function void main() {"
		),
		DataRow(
			"""
				else {              // There is no else keyword in the Square files.
			""",
			"else {"
		)
	]
	public void PassingSingleLineComments_RemovesThem(string input, string expected)
	{
		string actual = CompilerRegex.RemoveAllComments(input);

		// Normalize for test comparison — whitespace differences are irrelevant
		string normalizedActual = actual.Trim();
		string normalizedExpected = expected.Trim();

		Assert.AreEqual(normalizedActual, normalizedExpected, "Single-line comments were not removed as expected.");
	}

	[TestMethod()]
	[DataRow(
		"""
			/** Expressionless version of projects/10/Square/Main.jack. */
		""",
		""
	)]
	[DataRow(
		"""
			function void main() {
				var SquareGame game; /* HELLO! */
				let game = game;    
				do game.run();
				do game.dispose();
				return;
			}
		""",
		"""
			function void main() {
				var SquareGame game; 
				let game = game;    
				do game.run();
				do game.dispose();
				return;
			}
		"""
	)]
	public void PassingMultiLineComments_RemovesThem(string input, string expected)
	{
		string actual = CompilerRegex.RemoveAllComments(input);

		// Normalize for test comparison — whitespace differences are irrelevant
		string normalizedActual = actual.Trim();
		string normalizedExpected = expected.Trim();

		Assert.AreEqual(normalizedActual, normalizedExpected, "Multi-line comments were not removed as expected.");
	}

	[DataRow("")]
	[DataRow("class Main {")]
	[DataRow(
		"""
			function void main() {
				var SquareGame game;
				let game = game;    
				do game.run();
				do game.dispose();
				return;
			}
		"""
	)]
	public void PassingCodeWithoutComments_ReturnsJustCopy(string withoutComments)
	{
		string actual = CompilerRegex.RemoveAllComments(withoutComments);

		Assert.AreEqual(withoutComments, actual, "Code without comments should stay unmodified, since there are no comments.");
	}
}