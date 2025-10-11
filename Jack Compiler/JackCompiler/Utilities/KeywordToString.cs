namespace JackCompiler;

internal static class KeywordToString
{
	public static string ConvertToLower(Keyword keyword)
	{
		return keyword.ToString().ToLower();
	}
}