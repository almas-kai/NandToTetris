using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler.Utilities;

internal static class KeywordToString
{
	public static string ConvertToLower(Keyword keyword)
	{
		return keyword.ToString().ToLower();
	}
}