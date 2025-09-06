using System.Xml.Linq;
using JackCompiler;

namespace JackCompilerTests;

[TestClass()]
public class JackTokenizerTests
{
    [TestMethod()]
    [DataRow(@"./Assets/ArrayTestFiles/Main.jack", @"./Assets/ArrayTestFiles/ValidOutput/MainT.xml")]
    [DataRow(@"./Assets/ExpressionLessSquare/Main.jack", @"./Assets/ExpressionLessSquare/ValidOutput/MainT.xml")]
    [DataRow(@"./Assets/ExpressionLessSquare/Square.jack", @"./Assets/ExpressionLessSquare/ValidOutput/SquareT.xml")]
    [DataRow(@"./Assets/ExpressionLessSquare/SquareGame.jack", @"./Assets/ExpressionLessSquare/ValidOutput/SquareGameT.xml")]
    [DataRow(@"./Assets/Square/Main.jack", @"./Assets/Square/ValidOutput/MainT.xml")]
    [DataRow(@"./Assets/Square/Square.jack", @"./Assets/Square/ValidOutput/SquareT.xml")]
    [DataRow(@"./Assets/Square/SquareGame.jack", @"./Assets/Square/ValidOutput/SquareGameT.xml")]
    public void GenerateTestingXMLFile_PassingValidInput_GeneratesCorrectOutput(string inputPath, string validFilePath)
    {
        bool expected = true;
        FileInfo inputFile = new FileInfo(inputPath);
        JackTokenizer jackTokenizer = new JackTokenizer(inputFile);
        string outputPath = Path.Combine(
            Path.GetDirectoryName(validFilePath)!,
            Path.GetFileNameWithoutExtension(validFilePath) + "Output.xml"
        );

        jackTokenizer.GenerateTestingXMLFile(outputPath);
        XDocument validFile = XDocument.Load(validFilePath);
        XDocument outputFile = XDocument.Load(outputPath);
        bool actual = XNode.DeepEquals(validFile, outputFile);

        Assert.AreEqual(expected, actual);
    }
}