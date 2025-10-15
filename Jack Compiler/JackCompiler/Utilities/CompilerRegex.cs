using System.Text.RegularExpressions;

namespace JackCompiler.Utilities;

internal static class CompilerRegex
{
	private static readonly Regex _commentsRegex = new Regex(@"//.*|/\*[\s\S]*?\*/", RegexOptions.Compiled);
	private static readonly Regex _keywordRegex = new Regex(@"\b(class|constructor|function|method|field|static|var|int|char|boolean|void|true|false|null|this|let|do|if|else|while|return)\b", RegexOptions.Compiled);
	private static readonly Regex _symbolRegex = new Regex(@"[{}()\[\].,;+\-*/&|<>=~]", RegexOptions.Compiled);
	private static readonly Regex _integerConstantRegex = new Regex(@"\b\d+\b", RegexOptions.Compiled);
	private static readonly Regex _stringConstantRegex = new Regex(@"(""[^""]*"")", RegexOptions.Compiled);
	private static readonly Regex _identifierRegex = new Regex(@"\b([_a-zA-Z]+[_a-zA-Z0-9]*)\b", RegexOptions.Compiled);
	private static readonly Regex _spaceRegex = new Regex(@"\s", RegexOptions.Compiled);
	public static string RemoveAllComments(string instruction)
	{
		return _commentsRegex.Replace(instruction, string.Empty);
	}
	public static Match IsKeyword(string instruction)
	{
		return _keywordRegex.Match(instruction);
	}
	public static Match IsSymbol(string instruction)
	{
		return _symbolRegex.Match(instruction);
	}
	public static Match IsIntegerConstant(string instruction)
	{
		return _integerConstantRegex.Match(instruction);
	}
	public static Match IsStringConstant(string instruction)
	{
		return _stringConstantRegex.Match(instruction);
	}
	public static Match IsIdentifier(string instruction)
	{
		return _identifierRegex.Match(instruction);
	}
	public static Match IsSpace(string instruction)
	{
		return _spaceRegex.Match(instruction);
	}
}