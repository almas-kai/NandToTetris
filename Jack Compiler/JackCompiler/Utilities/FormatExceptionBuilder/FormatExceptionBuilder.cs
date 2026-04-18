using System.Text;

using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler.Utilities;

internal class FormatExceptionBuilder
{
    private List<string> _errorMessages { get; set; }
    public FormatExceptionBuilder()
    {
        _errorMessages = new List<string>();
    }

    private void Reset()
    {
        _errorMessages.Clear();
    }

    public FormatExceptionBuilder AddUnexpected(TokenType tokenType)
    {
        _errorMessages.Add($"Unexpected token type of \"{tokenType.ToLowerString()}\".");

        return this;
    }

    public FormatExceptionBuilder AddUnexpected(TokenType tokenType, string value)
    {
        AddUnexpected(tokenType);
        _errorMessages.Add($"Unexpected value of \"{value}\".");

        return this;
    }

    public FormatExceptionBuilder AddExpected(TokenType tokenType)
    {
        _errorMessages.Add($"Expected token type of \"{tokenType.ToLowerString()}\".");

        return this;
    }

    public FormatExceptionBuilder AddExpected(TokenType tokenType, params string[] values)
    {
        AddExpected(tokenType);
        string message = $"Expected value of \"{values[0]}\"";

        for(int i = 1; i < values.Length; i ++)
        {
            message += $", or \"{values[i]}\"";
        }

        _errorMessages.Add(message + ".");

        return this;
    }

    public FormatExceptionBuilder AddExpected(TokenType tokenType, params Keyword[] value)
    {
        AddExpected(
            tokenType,
            value.Select((keyword) => keyword.ToLowerString())
                .ToArray()
        );

        return this;
    }

    public FormatException Build()
    {
        var exception = new FormatException(string.Join(" ", _errorMessages));
        Reset();

        return exception;
    }
}