using System.Text.RegularExpressions;

using JackCompiler.Utilities;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler;

internal class JackTokenizer
{
    private static readonly (TokenType, Func<string, Match>)[] s_tokenTypeMatchers =
    {
        (TokenType.KEYWORD, CompilerRegex.IsKeyword),
        (TokenType.SYMBOL, CompilerRegex.IsSymbol),
        (TokenType.INT_CONST, CompilerRegex.IsIntegerConstant),
        (TokenType.STRING_CONST, CompilerRegex.IsStringConstant),
        (TokenType.IDENTIFIER, CompilerRegex.IsIdentifier)
    };
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

        foreach((TokenType Type, Func<string, Match> Match) tokenTypeMatcher in s_tokenTypeMatchers)
        {
            Match tempMatch = tokenTypeMatcher.Match(instruction);

            if (tempMatch.Success && tempMatch.Index is 0)
            {
                tokenType = tokenTypeMatcher.Type;
                tokenValue = NormalizeTokenValue(tokenType, tempMatch.Value);
                match = tempMatch;

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

    private static string NormalizeTokenValue(TokenType type, string rawValue) => type switch
    {
        TokenType.SYMBOL => rawValue switch
        {
            "<" => "&lt;",
            ">" => "&gt;",
            "&" => "&amp;",
            "\"" => "&quot;",
            _ => rawValue
        },
        TokenType.STRING_CONST => rawValue.Trim('"'),
        _ => rawValue
    };
}