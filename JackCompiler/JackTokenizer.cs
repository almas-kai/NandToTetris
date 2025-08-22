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

class JackTokenizer
{
	private string[] _instructions;
	private int _pointer = 0;
	private int _offset = 0;
	private string _currentInstruction = String.Empty;
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
			throw new InvalidOperationException($"Tokenizer error - Advance method error. Cannot advance. There are no more instructions. Current instruction number: {_pointer}. Current instruction: {_instructions[_pointer]}.");
		}

		SpaceToken:
		if (_offset == _currentInstruction.Length)
		{
			_Next();
		}
		else if (HasMoreTokens)
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
				throw new InvalidOperationException($"Tokenizer error - Advance method error. Couldn't match the token. Unrecognized token type. Current instruction number: {_pointer}. Current instruction: {_instructions[_pointer]}.");
			}
		}
	}
	public TokenType GetTokenType()
	{
		throw new NotImplementedException();
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

		foreach (Func<Match> matcher in matchers)
		{
			Match tempMatch = matcher.Invoke();
			if (tempMatch.Success && tempMatch.Index == _offset)
			{
				match = tempMatch;
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