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

    [TestMethod()]
    [DataRow(0, TokenType.KEYWORD, "class")]
    [DataRow(1, TokenType.IDENTIFIER, "Main")]
    [DataRow(139, TokenType.SYMBOL, "}")]
    [DataRow(3, TokenType.KEYWORD, "function")]
    [DataRow(104, TokenType.IDENTIFIER, "i")]
    [DataRow(89, TokenType.SYMBOL, "<")]
    [DataRow(86, TokenType.KEYWORD, "while")]
    [DataRow(137, TokenType.SYMBOL, ";")]
    [DataRow(73, TokenType.INT_CONST, "1")]
    [DataRow(65, TokenType.STRING_CONST, "ENTER THE NEXT NUMBER: ")]
    public void Peek_UsingPeekOnArrayTestFiles_WorksAsExpected(int instructionIndexAfterWhichWePeek, int expectedTokenType, string expectedTokenValue)
    {
        (TokenType, string) expected = ((TokenType)expectedTokenType, expectedTokenValue);
        FileInfo fileInfo = new FileInfo(@"./Assets/ArrayTestFiles/Main.jack");
        JackTokenizer jackTokenizer = new JackTokenizer(fileInfo);

        for (int i = 0; i < instructionIndexAfterWhichWePeek; i++)
        {
            jackTokenizer.Advance();
        }
        (TokenType type, string value) actual = jackTokenizer.Peek();

        Assert.AreEqual(expected, actual);
    }
}