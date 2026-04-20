using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler.Utilities;

internal static class EnumExtensions
{
    public static string ToLowerString<T>(this T enumType)
        where T: struct, Enum
    {
        return enumType.ToString().ToLower();
    }
    public static bool IsClassVarDec(this Keyword keyword)
    {
        return keyword is Keyword.FIELD or Keyword.STATIC;
    }
    public static bool IsSubroutineDec(this Keyword keyword)
    {
        return keyword is Keyword.CONSTRUCTOR or Keyword.FUNCTION or Keyword.METHOD;
    }
}