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
						Keyword keyword = jackTokenizer.GetKeyword();
						writer.WriteLine($"<keyword> {KeywordToString.ConvertToLower(keyword)} </keyword>");
						break;
					case TokenType.SYMBOL:
						string symbol = jackTokenizer.GetSymbol();
						writer.WriteLine($"<symbol> {symbol} </symbol>");
						break;
					case TokenType.IDENTIFIER:
						string identifier = jackTokenizer.GetIdentifier();
						writer.WriteLine($"<identifier> {identifier} </identifier>");
						break;
					case TokenType.INT_CONST:
						int intConstant = jackTokenizer.GetUInt15Constant();
						writer.WriteLine($"<integerConstant> {intConstant} </integerConstant>");
						break;
					case TokenType.STRING_CONST:
						string stringConstant = jackTokenizer.GetString();
						writer.WriteLine($"<stringConstant> {stringConstant} </stringConstant>");
						break;
					default:
						throw new FormatException($"Tokenizer error. Cannot generate testing XML file. Unrecognized token type: \"{type}\".");
				}
			}
			writer.WriteLine("</tokens>");
		}
	}
}