using System.Xml.Linq;

using JackCompiler;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompilerTests;

[TestClass()]
public class CompilationEngineTests
{
    [TestMethod()]
    [DataRow("./Assets/TDD/Class Tests/Simple Class Test/Input.jack")]
    public void InitializeInstance_PassingCorrectInput_GeneratedXMLFileExists(string inputPath)
    {
        bool expected = true;
        FileInfo inputFile = new FileInfo(inputPath);
        JackTokenizer jackTokenizer = new JackTokenizer(inputFile);
        string outputFilePath = inputFile.FullName.Replace(
          FileExtensions.JACK_EXTENSION,
          "-ACTUAL" + FileExtensions.XML_EXTENSION
        );

        new CompilationEngine(jackTokenizer, outputFilePath);

        bool actual = File.Exists(outputFilePath);

        Assert.AreEqual(expected, actual, "Compilation engine should generate an output XML file.");
    }

    [TestMethod()]
    [DataRow("./Assets/TDD/Class Tests/Simple Class Test/Input.jack", "./Assets/TDD/Class Tests/Simple Class Test/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With One Field/Input.jack", "./Assets/TDD/Class Tests/Class With One Field/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With Multiple Fields/Input.jack", "./Assets/TDD/Class Tests/Class With Multiple Fields/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With Empty Methods/Input.jack", "./Assets/TDD/Class Tests/Class With Empty Methods/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With Methods That Contain Vars/Input.jack", "./Assets/TDD/Class Tests/Class With Methods That Contain Vars/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With Let Statements/Input.jack", "./Assets/TDD/Class Tests/Class With Let Statements/Expected.xml")]
    public void InitializeInstance_PassingCorrectInput_GeneratesCorrectOutput(string inputFilePath, string expectedFilePath)
    {
        bool expected = true;
        FileInfo inputFile = new FileInfo(inputFilePath);
        JackTokenizer jackTokenizer = new JackTokenizer(inputFile);
        string outputFilePath = inputFile.FullName.Replace(
          FileExtensions.JACK_EXTENSION,
          "-ACTUAL" + FileExtensions.XML_EXTENSION
        );

        new CompilationEngine(jackTokenizer, outputFilePath);

        XDocument validFile = XDocument.Load(expectedFilePath);
        XDocument outputFile = XDocument.Load(outputFilePath);
        bool actual = XNode.DeepEquals(validFile, outputFile);

        Assert.AreEqual(expected, actual, "Generated tokenized output weren't equal to the expected one.");
    }
}