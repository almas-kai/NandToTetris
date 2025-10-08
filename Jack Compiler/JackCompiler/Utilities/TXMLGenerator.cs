namespace JackCompiler;

internal static class TXMLGenerator
{
	public static void GenerateTestingXMLFile(string fullPath)
	{
		using (StreamWriter writer = new StreamWriter(fullPath))
		{
			writer.WriteLine("<tokens>");
			Advance();
			while (HasMoreTokens)
			{
				TokenType type = CurrentToken.Type;
				switch (type)
				{
					case TokenType.KEYWORD:
						Keyword keyword = GetKeyword();
						writer.WriteLine($"<keyword> {keyword.ToString().ToLower()} </keyword>");
						break;
					case TokenType.SYMBOL:
						string symbol = GetSymbol();
						writer.WriteLine($"<symbol> {symbol} </symbol>");
						break;
					case TokenType.IDENTIFIER:
						string identifier = GetIdentifier();
						writer.WriteLine($"<identifier> {identifier} </identifier>");
						break;
					case TokenType.INT_CONST:
						int intConstant = GetInteger();
						writer.WriteLine($"<integerConstant> {intConstant} </integerConstant>");
						break;
					case TokenType.STRING_CONST:
						string stringConstant = GetString();
						writer.WriteLine($"<stringConstant> {stringConstant} </stringConstant>");
						break;
					default:
						throw new FormatException($"Tokenizer error. Cannot generate testing XML file. Unrecognized token type: \"{type}\".");
				}
				Advance();
			}
			writer.WriteLine("</tokens>");
		}
	}
}