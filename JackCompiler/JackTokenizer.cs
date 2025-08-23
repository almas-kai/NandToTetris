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
	public JackTokenizer(FileInfo fileInfo)
	{
		string patternForComments = @"//.*|/\*[\s\S]*?\*/";
		using (StreamReader streamReader = fileInfo.OpenText())
		{
			_instructions = Regex.Replace(streamReader.ReadToEnd(), patternForComments, "")
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
			return intConst;
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
		string keywordPattern = @"\b(class|constructor|function|method|field|static|var|int|char|boolean|void|true|false|null|this|let|do|if|else|while|return)\b";

		Regex keyword = new Regex(keywordPattern);

		return keyword.Match(_currentInstruction, _offset);
	}

	private Match _IsSymbol()
	{
		string symbolPattern = @"[{}()\[\].,;+\-*/&|<>=~]";

		Regex symbol = new Regex(symbolPattern);

		return symbol.Match(_currentInstruction, _offset);
	}

	private Match _IsIntegerConstant()
	{
		string integerConstantPattern = @"\b\d+\b";

		Regex integerConstant = new Regex(integerConstantPattern);

		return integerConstant.Match(_currentInstruction, _offset);
	}

	private Match _IsStringConstant()
	{
		string stringConstantPattern = @"(""[^""]*"")";

		Regex stringConstant = new Regex(stringConstantPattern);

		return stringConstant.Match(_currentInstruction, _offset);
	}

	private Match _IsIdentifier()
	{
		string identifierPattern = @"\b([_a-zA-Z]+[_a-zA-Z0-9]*)\b";

		Regex identifier = new Regex(identifierPattern);

		return identifier.Match(_currentInstruction, _offset);
	}

	private Match _IsSpace()
	{
		string spacePattern = @"\s";

		Regex space = new Regex(spacePattern);

		return space.Match(_currentInstruction, _offset);
	}
}