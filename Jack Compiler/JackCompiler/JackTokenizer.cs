using System.Text.RegularExpressions;

using JackCompiler.Utilities;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler;

internal class JackTokenizer
{
    private readonly Queue<string> _instructionsQueue;
    private string _currentInstruction = string.Empty;
    public (TokenType Type, string RawValue) CurrentToken { get; private set; }
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
            throw new InvalidOperationException("Cannot advance. There are no more instructions.");
        }

    SpaceLabel:
        if (_currentInstruction == string.Empty)
        {
            _currentInstruction = _instructionsQueue.Dequeue();
        }
        Match tokenMatch = _MatchToken(_currentInstruction, out (TokenType type, string value) token);

        if (tokenMatch.Success && token.type is not TokenType.UNKNOWN)
        {
            _currentInstruction = _currentInstruction.Remove(0, tokenMatch.Value.Length);
            if (token.type == TokenType.SPACE)
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
            throw new InvalidOperationException($"Couldn't match the token. Unrecognized token type. Token type is: \"{token.type}\", token value is: \"{token.value}\".");
        }
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

        if (peekMatch.Success && token.type is not TokenType.UNKNOWN)
        {
            currentInstruction = currentInstruction.Remove(0, peekMatch.Value.Length);

            if (token.type == TokenType.SPACE)
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
            throw new InvalidOperationException($"Couldn't peek. Unrecognized token type. Token type is \"{token.type}\", and token value is \"{token.value}\".");
        }
    }

    private Match _MatchToken(string instruction, out (TokenType type, string value) token)
    {
        Match? match = null;
        string tokenValue = string.Empty;
        TokenType tokenType = TokenType.UNKNOWN;
        Func<string, Match>[] tokenTypeMatchers = {
            CompilerRegex.IsKeyword,
            CompilerRegex.IsSymbol,
            CompilerRegex.IsIntegerConstant,
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
                        switch (tokenValue)
                        {
                            case "<":
                                tokenValue = "&lt;";
                                break;
                            case ">":
                                tokenValue = "&gt;";
                                break;
                            case "&":
                                tokenValue = "&amp;";
                                break;
                            case "\"":
                                tokenValue = "&quot;";
                                break;
                        }
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
                        throw new InvalidOperationException("Unrecognized token type.");
                }
                break;
            }
        }

        if (match is null)
        {
            match = CompilerRegex.IsSpace(instruction);
            tokenValue = match.Success ? match.Value : string.Empty;
            tokenType = match.Success ? TokenType.SPACE : TokenType.UNKNOWN;
        }

        token = (tokenType, tokenValue);

        return match;
    }
}