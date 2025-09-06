using System.Xml.Linq;
using JackCompiler;

namespace JackCompilerTests;

[TestClass()]
public class JackTokenizerTests
{
    [TestMethod()]
    [DataRow(@"./Assets/ArrayTestFiles/Main.jack", @"./Assets/ArrayTestFiles/ValidOutput/MainT.xml")]
    public void GenerateTestingXMLFile_PassingValidInput_GeneratesCorrectOutput(string inputPath, string validFilePath)
    {
        bool expected = true;
        FileInfo inputFile = new FileInfo(inputPath);
        JackTokenizer jackTokenizer = new JackTokenizer(inputFile);
        string outputPath = Path.Combine(
            Path.GetDirectoryName(validFilePath)!,
            Path.GetFileNameWithoutExtension(validFilePath) + "T.xml"
        );

        jackTokenizer.GenerateTestingXMLFile(outputPath);
        XDocument validFile = XDocument.Load(validFilePath);
        XDocument outputFile = XDocument.Load(outputPath);
        bool actual = XNode.DeepEquals(validFile, outputFile);

        Assert.AreEqual(expected, actual);
    }
}