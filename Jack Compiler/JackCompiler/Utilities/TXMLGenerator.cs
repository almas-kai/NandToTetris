using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler.Utilities;

internal static class TXMLGenerator
{
    public static void GenerateTestingXMLFile(FileInfo jackFile, string outputTokenFile)
    {
        if (JackFileReader.IsCorrectFilePath(jackFile.FullName) is false)
        {
            throw new InvalidOperationException("The provided file is not a '.jack' file, or the file doesn't exist.");
        }

        JackTokenizer jackTokenizer = new JackTokenizer(jackFile);

        using (StreamWriter writer = new StreamWriter(outputTokenFile))
        {
            writer.WriteLine("<tokens>");
            while (jackTokenizer.HasMoreTokens)
            {
                jackTokenizer.Advance();
                TokenType type = jackTokenizer.CurrentToken.Type;
                switch (type)
                {
                    case TokenType.KEYWORD:
                        writer.WriteLine($"<keyword> {jackTokenizer.GetKeyword().ToLowerString()} </keyword>");
                        break;
                    case TokenType.SYMBOL:
                        writer.WriteLine($"<symbol> {jackTokenizer.GetSymbol()} </symbol>");
                        break;
                    case TokenType.IDENTIFIER:
                        writer.WriteLine($"<identifier> {jackTokenizer.GetIdentifier()} </identifier>");
                        break;
                    case TokenType.INT_CONST:
                        writer.WriteLine($"<integerConstant> {jackTokenizer.GetUInt15Constant()} </integerConstant>");
                        break;
                    case TokenType.STRING_CONST:
                        writer.WriteLine($"<stringConstant> {jackTokenizer.GetString()} </stringConstant>");
                        break;
                    default:
                        throw new FormatException($"Cannot generate testing XML file. Unrecognized token type: \"{type}\".");
                }
            }
            writer.WriteLine("</tokens>");
        }
    }
}