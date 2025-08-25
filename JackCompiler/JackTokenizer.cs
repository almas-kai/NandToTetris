using System.Text.RegularExpressions;

namespace JackCompiler;

public enum TokenType
{
	KEYWORD,
	SYMBOL,
	IDENTIFIER,
	INT_CONST,
	STRING_CONST
}

public enum Keyword
{
	CLASS,
	METHOD,
	FUNCTION,
	CONSTRUCTOR,
	INT,
	BOOLEAN,
	CHAR,
	VOID,
	VAR,
	STATIC,
	FIELD,
	LET,
	DO,
	IF,
	ELSE,
	WHILE,
	RETURN,
	TRUE,
	FALSE,
	NULL,
	THIS
}

class JackTokenizer
{
	private string[] _instructions;
	private int _pointer = 0;
	private int _offset = 0;
	private string _currentInstruction = String.Empty;
	private string _currentTokenValue = String.Empty;
	private TokenType _currentTokenType;

	private static readonly Regex _commentsRegex = new Regex(@"//.*|/\*[\s\S]*?\*/", RegexOptions.Compiled);
	private static readonly Regex _keywordRegex = new Regex(@"\b(class|constructor|function|method|field|static|var|int|char|boolean|void|true|false|null|this|let|do|if|else|while|return)\b", RegexOptions.Compiled);
	private static readonly Regex _symbolRegex = new Regex(@"[{}()\[\].,;+\-*/&|<>=~]", RegexOptions.Compiled);
	private static readonly Regex _integerConstantRegex = new Regex(@"\b\d+\b", RegexOptions.Compiled);
	private static readonly Regex _stringConstantRegex = new Regex(@"(""[^""]*"")", RegexOptions.Compiled);
	private static readonly Regex _identifierRegex = new Regex(@"\b([_a-zA-Z]+[_a-zA-Z0-9]*)\b", RegexOptions.Compiled);
	private static readonly Regex _spaceRegex = new Regex(@"\s", RegexOptions.Compiled);

	public JackTokenizer(FileInfo fileInfo)
	{
		using (StreamReader streamReader = fileInfo.OpenText())
		{
			_instructions = _commentsRegex.Replace(streamReader.ReadToEnd(), "")
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
			Match tokenMatch = _MatchToken();
			if (tokenMatch.Success)
			{
				_offset += tokenMatch.Value.Length;
				if (tokenMatch.Value == " ")
				{
					goto SpaceToken;
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
		return _currentTokenType;
	}
	public Keyword GetKeyword()
	{
		if (_currentTokenType is not TokenType.KEYWORD)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the keyword. Because token type is not keyword. It is: \"{_currentTokenType}\".");
		}

		if (Enum.TryParse<Keyword>(_currentTokenValue, ignoreCase: true, out Keyword keyword))
		{
			return keyword;
		}
		else
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the keyword. There is no such keyword as \"{_currentTokenValue}\".");
		}
	}
	public char GetSymbol()
	{
		if (_currentTokenType is not TokenType.SYMBOL)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the symbol. Because token type is not symbol. It is: \"{_currentTokenType}\".");
		}

		if (_currentTokenValue.Length == 1)
		{
			return _currentTokenValue[0];
		}
		else
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the symbol. Because token value is string: \"{_currentTokenValue}\".");
		}
	}
	public string GetIdentifier()
	{
		if (_currentTokenType is not TokenType.IDENTIFIER)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the identifier. Because token type is not identifier. It is: \"{_currentTokenType}\".");
		}

		return _currentTokenValue;
	}
	public int GetInteger()
	{
		if (_currentTokenType is not TokenType.INT_CONST)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the integer constant. Because token type is not integer constant. It is: \"{_currentTokenType}\".");
		}

		if (int.TryParse(_currentTokenValue, out int intConst))
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
			throw new InvalidOperationException($"Tokenizer error. Cannot get the integer constant. Because it is not an integer constant. It is: \"{_currentTokenValue}\".");
		}
	}
	public string GetString()
	{
		if (_currentTokenType is not TokenType.STRING_CONST)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the string constant. Because token type is not string constant. It is: \"{_currentTokenType}\".");
		}

		return _currentTokenValue;
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
				TokenType tokenType = GetTokenType();
				switch (tokenType)
				{
					case TokenType.KEYWORD:
						Keyword keyword = GetKeyword();
						writer.WriteLine($"<keyword> {keyword.ToString().ToLower()} </keyword>");
						break;
					case TokenType.SYMBOL:
						char symbol = GetSymbol();
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

	private Match _MatchToken()
	{
		Match? match = null;
		Func<Match>[] matchers = new Func<Match>[] {
			_IsKeyword,
			_IsSymbol,
			_IsIntegerConstant,
			_IsStringConstant,
			_IsIdentifier
		};

		for (int i = 0; i < matchers.Length; i++)
		{
			Func<Match> matcher = matchers[i];
			Match tempMatch = matcher.Invoke();
			if (tempMatch.Success && tempMatch.Index == _offset)
			{
				match = tempMatch;
				_currentTokenValue = tempMatch.Value;
				switch (i)
				{
					case 0:
						_currentTokenType = TokenType.KEYWORD;
						break;
					case 1:
						_currentTokenType = TokenType.SYMBOL;
						break;
					case 2:
						_currentTokenType = TokenType.INT_CONST;
						break;
					case 3:
						_currentTokenType = TokenType.STRING_CONST;
						_currentTokenValue = _currentTokenValue.Trim('"');
						break;
					case 4:
						_currentTokenType = TokenType.IDENTIFIER;
						break;
					default:
						throw new InvalidOperationException($"Tokenizer error. Error in _MatchToken. Unrecognized token type.");
				}
				break;
			}
		}

		if (match is null)
		{
			match = _IsSpace();
		}

		return match;
	}

	private Match _IsKeyword()
	{
		return _keywordRegex.Match(_currentInstruction, _offset);
	}

	private Match _IsSymbol()
	{
		return _symbolRegex.Match(_currentInstruction, _offset);
	}

	private Match _IsIntegerConstant()
	{

		return _integerConstantRegex.Match(_currentInstruction, _offset);
	}

	private Match _IsStringConstant()
	{
		return _stringConstantRegex.Match(_currentInstruction, _offset);
	}

	private Match _IsIdentifier()
	{
		return _identifierRegex.Match(_currentInstruction, _offset);
	}

	private Match _IsSpace()
	{
		return _spaceRegex.Match(_currentInstruction, _offset);
	}
}