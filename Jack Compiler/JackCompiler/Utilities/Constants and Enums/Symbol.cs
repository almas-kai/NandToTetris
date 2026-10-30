namespace JackCompiler.Utilities.ConstantsAndEnums;

internal static class Symbol
{
    public const string OPENING_BRACE = "{";
    public const string CLOSING_BRACE = "}";
    public const string OPENING_PARENTHESIS = "(";
    public const string CLOSING_PARENTHESIS = ")";
    public const string OPENING_BRACKET = "[";
    public const string CLOSING_BRACKET = "]";
    public const string SEMICOLON = ";";
    public const string COMMA = ",";
    public const string EQUAL = "=";
    public const string DOT = ".";
    public const string PLUS = "+";
    public const string MINUS = "-";
    public const string MULTIPLICATION = "*";
    public const string DIVISION = "/";
    public const string AMPERSAND = "&amp;";
    public const string PIPE = "|";
    public const string LESS_THAN = "&lt;";
    public const string GREATER_THAN = "&gt;";
    public const string QUOTE = "&quot;";
    public const string TILDE = "~";
    private const string _LESS_THAN = "<";
    private const string _GREATER_THAN = ">";
    private const string _AMPERSAND = "&";
    private const string _QUOTE = "\"";

    public static bool IsValidOperator(string symbol)
    {
        return symbol is (
            PLUS or
            MINUS or
            MULTIPLICATION or
            DIVISION or
            AMPERSAND or
            PIPE or
            LESS_THAN or
            GREATER_THAN or
            EQUAL
        );
    }

    public static string NormalizeTokenValue(TokenType type, string rawValue) => type switch
    {
        TokenType.SYMBOL => rawValue switch
        {
            _LESS_THAN => LESS_THAN,
            _GREATER_THAN => GREATER_THAN,
            _AMPERSAND => AMPERSAND,
            _QUOTE => QUOTE,
            _ => rawValue
        },
        TokenType.STRING_CONST => rawValue.Trim(_QUOTE[0]),
        _ => rawValue
    };
}