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
    [DataRow("./Assets/TDD/Class Tests/Class With Simple Conditionals/Input.jack", "./Assets/TDD/Class Tests/Class With Simple Conditionals/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With Simple While Loop/Input.jack", "./Assets/TDD/Class Tests/Class With Simple While Loop/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With Simple Do Instruction/Input.jack", "./Assets/TDD/Class Tests/Class With Simple Do Instruction/Expected.xml")]
    [DataRow("./Assets/TDD/Class Tests/Class With Simple Return/Input.jack", "./Assets/TDD/Class Tests/Class With Simple Return/Expected.xml")]
    [DataRow("./Assets/ArrayTestFiles/Main.jack", "./Assets/ArrayTestFiles/ValidOutput/Main.xml")]
    [DataRow("./Assets/ExpressionLessSquare/Main.jack", "./Assets/ExpressionLessSquare/ValidOutput/Main.xml")]
    [DataRow("./Assets/ExpressionLessSquare/Square.jack", "./Assets/ExpressionLessSquare/ValidOutput/Square.xml")]
    [DataRow("./Assets/ExpressionLessSquare/SquareGame.jack", "./Assets/ExpressionLessSquare/ValidOutput/SquareGame.xml")]
    [DataRow("./Assets/Square/Main.jack", "./Assets/Square/ValidOutput/Main.xml")]
    [DataRow("./Assets/Square/Square.jack", "./Assets/Square/ValidOutput/Square.xml")]
    [DataRow("./Assets/Square/SquareGame.jack", "./Assets/Square/ValidOutput/SquareGame.xml")]
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