using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler.Utilities;

internal static class EnumToString<T> where T : Enum
{
	public static string ConvertToLower(T enumValue)
	{
		return enumValue.ToString().ToLower();
	}
}