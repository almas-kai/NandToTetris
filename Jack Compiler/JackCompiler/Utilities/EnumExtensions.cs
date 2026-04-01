namespace JackCompiler.Utilities;

internal static class EnumExtensions
{
    public static string ToLowerString<T>(this T enumType)
        where T: struct, Enum
    {
        return enumType.ToString().ToLower();
    }
}