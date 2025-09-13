using System.Text.RegularExpressions;

namespace JackCompiler;

public static class CompilerRegex
{
	private static readonly Regex _commentsRegex = new Regex(@"//.*|/\*[\s\S]*?\*/", RegexOptions.Compiled);
	private static readonly Regex _keywordRegex = new Regex(@"\b(class|constructor|function|method|field|static|var|int|char|boolean|void|true|false|null|this|let|do|if|else|while|return)\b", RegexOptions.Compiled);
	private static readonly Regex _symbolRegex = new Regex(@"[{}()\[\].,;+\-*/&|<>=~]", RegexOptions.Compiled);
	private static readonly Regex _integerConstantRegex = new Regex(@"\b\d+\b", RegexOptions.Compiled);
	private static readonly Regex _stringConstantRegex = new Regex(@"(""[^""]*"")", RegexOptions.Compiled);
	private static readonly Regex _identifierRegex = new Regex(@"\b([_a-zA-Z]+[_a-zA-Z0-9]*)\b", RegexOptions.Compiled);
	private static readonly Regex _spaceRegex = new Regex(@"\s", RegexOptions.Compiled);
	public static string ReplaceAllComments(string instruction, string replacement)
	{
		return _commentsRegex.Replace(instruction, replacement);
	}
	public static Match IsKeyword(string instruction, int offset)
	{
		return _keywordRegex.Match(instruction, offset);
	}
	public static Match IsSymbol(string instruction, int offset)
	{
		return _symbolRegex.Match(instruction, offset);
	}

	public static Match IsIntegerConstant(string instruction, int offset)
	{

		return _integerConstantRegex.Match(instruction, offset);
	}

	public static Match IsStringConstant(string instruction, int offset)
	{
		return _stringConstantRegex.Match(instruction, offset);
	}

	public static Match IsIdentifier(string instruction, int offset)
	{
		return _identifierRegex.Match(instruction, offset);
	}

	public static Match IsSpace(string instruction, int offset)
	{
		return _spaceRegex.Match(instruction, offset);
	}
}