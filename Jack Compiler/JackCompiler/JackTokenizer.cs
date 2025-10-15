using System.Text.RegularExpressions;
using JackCompiler.Utilities;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler;

internal class JackTokenizer
{
	private Queue<string> _instructionsQueue;
	private string _currentInstruction = string.Empty;
	public (TokenType Type, string Value) CurrentToken { get; private set; }
	public bool HasMoreTokens => _instructionsQueue.Count > 0 || _currentInstruction != string.Empty;

	public JackTokenizer(FileInfo fileInfo)
	{
		using (StreamReader streamReader = fileInfo.OpenText())
		{
			string rawInstructions = streamReader.ReadToEnd();

			_instructionsQueue = new Queue<string>
			(
				CompilerRegex.RemoveAllComments(rawInstructions)
					.Split("\n")
					.Select(instruction => instruction.Trim())
					.Where(instruction => instruction != string.Empty)
			);
		}
	}
	public void Advance()
	{
		if (HasMoreTokens is false)
		{
			throw new InvalidOperationException("Tokenizer error - Advance method error. Cannot advance. There are no more instructions.");
		}

	SpaceLabel:
		if (_currentInstruction == string.Empty)
		{
			_currentInstruction = _instructionsQueue.Dequeue();
		}
		Match tokenMatch = _MatchToken(_currentInstruction, out (TokenType type, string value) token);

		if (tokenMatch.Success)
		{
			_currentInstruction = _currentInstruction.Remove(0, tokenMatch.Value.Length);
			if (tokenMatch.Value == " ")
			{
				goto SpaceLabel;
			}
			else
			{
				CurrentToken = token;
			}
		}
		else
		{
			throw new InvalidOperationException("Tokenizer error - Advance method error. Couldn't match the token. Unrecognized token type.");
		}
	}
	public Keyword GetKeyword()
	{
		if (CurrentToken.Type is not TokenType.KEYWORD)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the keyword. Because token type is not keyword. The token type is \"{CurrentToken.Type}\".");
		}

		bool isValidKeyword = Enum.TryParse<Keyword>(
			value: CurrentToken.Value,
			ignoreCase: true,
			result: out Keyword keyword
		);

		if (!isValidKeyword)
		{
			throw new InvalidOperationException($"Tokenizer error. Unrecognized keyword: \"{CurrentToken.Value}\".");
		}

		return keyword;
	}
	public string GetSymbol()
	{
		if (CurrentToken.Type is not TokenType.SYMBOL || CurrentToken.Value.Length != 1)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the symbol. Because token type is not symbol or the parsing instruction value is incorrect. The token type is \"{CurrentToken.Type}\", and token value is \"{CurrentToken.Value}\".");
		}

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
	public string GetIdentifier()
	{
		if (CurrentToken.Type is not TokenType.IDENTIFIER)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the identifier. Because token type is not identifier. The token type is \"{CurrentToken.Type}\".");
		}

		return CurrentToken.Value;
	}
	public int GetUInt15Constant()
	{
		if (CurrentToken.Type is not TokenType.INT_CONST)
		{
			throw new InvalidOperationException($"Tokenizer error. Cannot get the integer constant. Because token type is not integer constant. The token type is \"{CurrentToken.Type}\".");
		}

		if (int.TryParse(CurrentToken.Value, out int intConst))
		{
			if (intConst >= 0 && intConst <= 32767)
			{
				return intConst;
			}
			else
			{
				throw new InvalidOperationException($"Tokenizer error. Cannot get the integer constant. Because it has to be 0 <= x <= 32767. But it was: \"{intConst}\".");
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
			throw new InvalidOperationException($"Tokenizer error. Cannot get the string constant. Because token type is not a string constant. The token type is: \"{CurrentToken.Type}\".");
		}

		return CurrentToken.Value;
	}
	public (TokenType type, string value) Peek()
	{
		if (HasMoreTokens is false)
		{
			throw new InvalidOperationException("There are no instructions to peek.");
		}

		string currentInstruction = _currentInstruction;

	peekLabel:
		if (currentInstruction == string.Empty)
		{
			currentInstruction = _instructionsQueue.Peek();
		}

		Match peekMatch = _MatchToken(currentInstruction, out (TokenType type, string value) token);

		if (peekMatch.Success)
		{
			currentInstruction = currentInstruction.Remove(0, peekMatch.Value.Length);

			if (peekMatch.Value == " ")
			{
				goto peekLabel;
			}
			else
			{
				return (token.type, token.value);
			}
		}
		else
		{
			throw new InvalidOperationException("Tokenizer error. Couldn't peek. Unrecognized token type.");
		}
	}
	
	private Match _MatchToken(string instruction, out (TokenType type, string value) token)
	{
		Match? match = null;
		string tokenValue = string.Empty;
		TokenType tokenType = TokenType.UNKNOWN;
		Func<string, Match>[] tokenTypeMatchers = new Func<string, Match>[] {
			CompilerRegex.IsKeyword,
			CompilerRegex.IsSymbol,
			CompilerRegex.IsPositiveIntegerConstant,
			CompilerRegex.IsStringConstant,
			CompilerRegex.IsIdentifier
		};

		for (int i = 0; i < tokenTypeMatchers.Length; i++)
		{
			Func<string, Match> matcher = tokenTypeMatchers[i];
			Match tempMatch = matcher.Invoke(instruction);
			if (tempMatch.Success && tempMatch.Index == 0)
			{
				tokenValue = tempMatch.Value;
				match = tempMatch;
				switch (i)
				{
					case 0:
						tokenType = TokenType.KEYWORD;
						break;
					case 1:
						tokenType = TokenType.SYMBOL;
						break;
					case 2:
						tokenType = TokenType.INT_CONST;
						break;
					case 3:
						tokenType = TokenType.STRING_CONST;
						tokenValue = tokenValue.Trim('"');
						break;
					case 4:
						tokenType = TokenType.IDENTIFIER;
						break;
					default:
						throw new InvalidOperationException("Tokenizer error. Error in _MatchToken. Unrecognized token type.");
				}
				break;
			}
		}

		if (match is null)
		{
			match = CompilerRegex.IsSpace(instruction);
			tokenValue = match.Success ? match.Value : string.Empty;
		}

		token = (tokenType, tokenValue);

		return match;
	}
}