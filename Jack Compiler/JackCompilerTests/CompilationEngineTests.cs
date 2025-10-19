using JackCompiler;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompilerTests;

[TestClass()]
public class CompilationEngineTests
{
	[TestMethod()]
	[DataRow("./Assets/ArrayTestFiles/Main.jack")]
	public void InitializeInstance_PassingCorrectInput_GeneratedXMLFileExists(string inputPath)
	{
		bool expected = true;
		FileInfo inputFile = new FileInfo(inputPath);
		JackTokenizer jackTokenizer = new JackTokenizer(inputFile);
		string outputFilePath = inputFile.FullName.Replace(
			FileExtensions.JACK_EXTENSION,
			FileExtensions.XML_EXTENSION
		);

		new CompilationEngine(jackTokenizer, outputFilePath);

		bool actual = File.Exists(outputFilePath);

		Assert.AreEqual(expected, actual, "Compilation engine should generate an output XML file.");
	}
}