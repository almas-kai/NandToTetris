namespace JackCompiler.Utilities.ConstantsAndEnums;

// Don't forget to rename it to Symbol.
internal static class Symbols
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
    public const string TILDE = "~";

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
}