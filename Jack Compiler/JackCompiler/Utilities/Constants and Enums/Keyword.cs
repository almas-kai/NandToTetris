namespace JackCompiler.Utilities.ConstantsAndEnums;

internal static class Keyword
{
    public const string CLASS = "class";
    public const string FUNCTION = "function";
    public const string CONSTRUCTOR = "constructor";
    public const string METHOD = "method";
    public const string INT = "int";
    public const string BOOLEAN = "boolean";
    public const string CHAR = "char";
    public const string VOID = "void";
    public const string VAR = "var";
    public const string STATIC = "static";
    public const string FIELD = "field";
    public const string LET = "let";
    public const string DO = "do";
    public const string IF = "if";
    public const string ELSE = "else";
    public const string WHILE = "while";
    public const string RETURN = "return";
    public const string TRUE = "true";
    public const string FALSE = "false";
    public const string NULL = "null";
    public const string THIS = "this";

    public static bool IsClassVarDec(string keyword)
    {
        return keyword is (FIELD or STATIC);
    }

    public static bool IsSubroutineDec(string keyword)
    {
        return keyword is (CONSTRUCTOR or FUNCTION or METHOD);
    }

    public static bool IsStatementKeyword(string keyword)
    {
        return keyword is (LET or IF or WHILE or DO or RETURN);
    }

    public static bool IsKeywordConstant(string keyword)
    {
        return keyword is (TRUE or FALSE or NULL or THIS);
    }
}