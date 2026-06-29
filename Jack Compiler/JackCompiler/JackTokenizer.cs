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
        CurrentToken = GetNextToken(isDequeue: true);
    }
    public (TokenType type, string value) Peek()
    {
        return GetNextToken(isDequeue: false);
    }

    private (TokenType type, string value) GetNextToken(bool isDequeue)
    {
        string currentInstruction = _currentInstruction;

        while(HasMoreTokens)
        {
            if (currentInstruction == string.Empty)
            {
                currentInstruction = isDequeue ? _instructionsQueue.Dequeue() : _instructionsQueue.Peek();
            }

            Match tokenMatch = _MatchToken(currentInstruction, out (TokenType type, string value) token);

            if (!(tokenMatch.Success && token.type is not TokenType.UNKNOWN))
            {
                throw new InvalidOperationException($"Couldn't match the token. Unrecognized token type. Token type is: \"{token.type}\", token value is: \"{token.value}\".");
            }

            currentInstruction = currentInstruction.Remove(0, tokenMatch.Value.Length);

            if (token.type is not TokenType.SPACE)
            {
                if (isDequeue)
                {
                    _currentInstruction = currentInstruction;
                }

                return token;
            }
        }

        throw new InvalidOperationException("Cannot read the next instruction. It does not exist.");
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