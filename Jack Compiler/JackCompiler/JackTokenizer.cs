using System.Text.RegularExpressions;

namespace JackCompiler;

internal class JackTokenizer
{
	private string[] _instructions;
	private int _pointer = 0;
	private int _offset = 0;
	private string _currentInstruction = String.Empty;
	public (TokenType Type, string Value) CurrentToken { get; private set; }

	public JackTokenizer(FileInfo fileInfo)
	{
		using (StreamReader streamReader = fileInfo.OpenText())
		{
			string rawInstructions = streamReader.ReadToEnd();
			_instructions = CompilerRegex.ReplaceAllComments(rawInstructions, "")
				.Split("\n")
				.Select(instruction => instruction.Trim())
				.Where(instruction => instruction != String.Empty)
				.ToArray();
		}
		if (_instructions.Length > 0)
		{
			HasMoreTokens = true;
		}
	}
	public bool HasMoreTokens { get; private set; }
	public void Advance()
	{
		if (HasMoreTokens is false)
		{
			throw new InvalidOperationException($"Tokenizer error - Advance method error. Cannot advance. There are no more instructions. Current instruction number: \"{_pointer}\". Current instruction: \"{_instructions[_pointer]}\".");
		}

	SpaceToken:
		if (_offset == _currentInstruction.Length)
		{
			_Next();
		}
		if (HasMoreTokens)
		{
			Match tokenMatch = _MatchToken(_currentInstruction, _offset, out (TokenType type, string value) token);
			if (tokenMatch.Success)
			{
				_offset += tokenMatch.Value.Length;
				if (tokenMatch.Value == " ")
				{
					goto SpaceToken;
				}
				else
				{
					CurrentToken = token;
				}
			}
			else
			{
				throw new InvalidOperationException($"Tokenizer error - Advance method error. Couldn't match the token. Unrecognized token type. Current instruction number: \"{_pointer}\". Current instruction: \"{_instructions[_pointer]}\".");
			}
		}
	}
	public TokenType GetTokenType()
	{
		return CurrentToken.Type;
	}
	public Keyword GetKeyword()
	{
		if (CurrentToken.Type is not TokenType.KEYWORD)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the keyword. Because token type is not keyword. It is: \"{CurrentToken.Type}\".");
		}

		if (Enum.TryParse<Keyword>(CurrentToken.Value, ignoreCase: true, out Keyword keyword))
		{
			return keyword;
		}
		else
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the keyword. There is no such keyword as \"{CurrentToken.Value}\".");
		}
	}
	public string GetSymbol()
	{
		if (CurrentToken.Type is not TokenType.SYMBOL)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the symbol. Because token type is not symbol. It is: \"{CurrentToken.Type}\".");
		}

		if (CurrentToken.Value.Length == 1)
		{
			switch (CurrentToken.Value)
			{
				case "<":
					return "&lt;";
				case ">":
					return "&gt;";
				case "&":
					return "&amp;";
				case "\"":
					return "&quot;";
				default:
					return CurrentToken.Value;
			}
		}
		else
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the symbol. Because token value is string: \"{CurrentToken.Value}\".");
		}
	}
	public string GetIdentifier()
	{
		if (CurrentToken.Type is not TokenType.IDENTIFIER)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the identifier. Because token type is not identifier. It is: \"{CurrentToken.Type}\".");
		}

		return CurrentToken.Value;
	}
	public int GetInteger()
	{
		if (CurrentToken.Type is not TokenType.INT_CONST)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the integer constant. Because token type is not integer constant. It is: \"{CurrentToken.Type}\".");
		}

		if (int.TryParse(CurrentToken.Value, out int intConst))
		{
			if (intConst >= 0 && intConst <= 32767)
			{
				return intConst;
			}
			else
			{
				throw new InvalidOperationException($"Tokenizer error. Cannot get the integer constant. Because it has to be 0 <= x <= 32767. But it was: {intConst}.");
			}
		}
		else
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the integer constant. Because it is not an integer constant. It is: \"{CurrentToken.Value}\".");
		}
	}
	public string GetString()
	{
		if (CurrentToken.Type is not TokenType.STRING_CONST)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the string constant. Because token type is not string constant. It is: \"{CurrentToken.Type}\".");
		}

		return CurrentToken.Value;
	}
	public void GenerateTestingXMLFile(string fullPath)
	{
		if (_pointer != 0 || _offset != 0)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot generate testing XML file. The pointer and offset are not at the start. Pointer is {_pointer}, offset is {_offset}.");
		}
		using (StreamWriter writer = new StreamWriter(fullPath))
		{
			Advance();
			writer.WriteLine("<tokens>");
			while (HasMoreTokens)
			{
				TokenType Type = GetTokenType();
				switch (Type)
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
				}
				Advance();
			}
			writer.WriteLine("</tokens>");
		}
		_pointer = 0;
		_offset = 0;
		if (_pointer < _instructions.Length)
		{
			HasMoreTokens = true;
		}
	}
	private void _Next()
	{
		if (_pointer < _instructions.Length)
		{
			_currentInstruction = _instructions[_pointer];
			_offset = 0;
			_pointer++;
		}
		else
		{
			HasMoreTokens = false;
		}
	}

	private Match _MatchToken(string instruction, int offset, out (TokenType type, string value) token)
	{
		Match? match = null;
		string Value = String.Empty;
		TokenType Type = TokenType.KEYWORD;
		Func<string, int, Match>[] matchers = new Func<string, int, Match>[] {
			CompilerRegex.IsKeyword,
			CompilerRegex.IsSymbol,
			CompilerRegex.IsIntegerConstant,
			CompilerRegex.IsStringConstant,
			CompilerRegex.IsIdentifier
		};

		for (int i = 0; i < matchers.Length; i++)
		{
			Func<string, int, Match> matcher = matchers[i];
			Match tempMatch = matcher.Invoke(instruction, offset);
			if (tempMatch.Success && tempMatch.Index == offset)
			{
				match = tempMatch;
				Value = tempMatch.Value;
				switch (i)
				{
					case 0:
						Type = TokenType.KEYWORD;
						break;
					case 1:
						Type = TokenType.SYMBOL;
						break;
					case 2:
						Type = TokenType.INT_CONST;
						break;
					case 3:
						Type = TokenType.STRING_CONST;
						Value = Value.Trim('"');
						break;
					case 4:
						Type = TokenType.IDENTIFIER;
						break;
					default:
						throw new InvalidOperationException($"Tokenizer error. Error in _MatchToken. Unrecognized token type.");
				}
				break;
			}
		}

		if (match is null)
		{
			match = CompilerRegex.IsSpace(instruction, offset);
		}

		token = (Type, Value);

		return match;
	}
}