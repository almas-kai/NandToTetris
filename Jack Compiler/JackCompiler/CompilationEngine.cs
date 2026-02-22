using JackCompiler.Utilities;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler;

internal class CompilationEngine : IDisposable
{
    private readonly JackTokenizer _jackTokenizer;
    private readonly string _outputFileName;
    private readonly StreamWriter _outputWriter;
    private bool _isDisposed;
    public CompilationEngine(JackTokenizer jackTokenizer, string outputFileName)
    {
        _jackTokenizer = jackTokenizer;
        _outputFileName = outputFileName;
        _outputWriter = new StreamWriter(_outputFileName);
        CompileClass();
        Dispose();
    }
    private void CompileClass()
    {
        Write("<class>");

        _jackTokenizer.Advance();

        (TokenType type, string rawValue) tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not TokenType.KEYWORD)
        {
            throw new FormatException($"Token type of \"{TokenType.KEYWORD}\" was expected. But got token type of \"{tempTokenRef.type}\".");
        }
        else if (_jackTokenizer.GetKeyword() is not Keyword.CLASS)
        {
            throw new FormatException($"Keyword of \"{Keyword.CLASS}\" was expected. But got keyword of \"{_jackTokenizer.GetKeyword()}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
            EnumToString<Keyword>.ConvertToLower(
            _jackTokenizer.GetKeyword()
            )
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not TokenType.IDENTIFIER)
        {
            throw new FormatException($"The token type of \"{TokenType.IDENTIFIER}\" was expected. But got: \"{tempTokenRef.type}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
            _jackTokenizer.GetIdentifier()
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not "{")
        {
            throw new FormatException($"The token type of \"{TokenType.SYMBOL}\" with the value of \"{{\" was expected. But got the token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        while (tempTokenRef.type is TokenType.KEYWORD && (_jackTokenizer.GetKeyword() is Keyword.STATIC or Keyword.FIELD))
        {
            CompileClassVarDec();
            _jackTokenizer.Advance();
            tempTokenRef = _jackTokenizer.CurrentToken;
        }

        while (tempTokenRef.type is TokenType.KEYWORD && (_jackTokenizer.GetKeyword() is Keyword.FUNCTION or Keyword.CONSTRUCTOR or Keyword.METHOD))
        {
            CompileSubroutine();
            tempTokenRef = _jackTokenizer.CurrentToken;
        }

        if (tempTokenRef.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not "}")
        {
            throw new FormatException($"The token type of \"{TokenType.SYMBOL}\" with the value of \"}}\" was expected. But got the token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
            );
        }

        Write("</class>");
    }
    private void CompileClassVarDec()
    {
        Write("<classVarDec>");

        (TokenType type, string rawValue) tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not TokenType.KEYWORD || _jackTokenizer.GetKeyword() is not (Keyword.FIELD or Keyword.STATIC))
        {
            throw new FormatException($"Token type of \"{TokenType.KEYWORD}\" with the value of \"{Keyword.FIELD}\" or \"{Keyword.STATIC}\" was expected. But got token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
            EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not (TokenType.KEYWORD or TokenType.IDENTIFIER))
        {
            throw new FormatException($"Token type of \"{TokenType.KEYWORD}\" or \"{TokenType.IDENTIFIER}\" was expected. But got token type of \"{tempTokenRef.type}\".");
        }
        else if (tempTokenRef.type is TokenType.KEYWORD)
        {
            if (IsPrimitiveType(_jackTokenizer.GetKeyword()) is false)
            {
                throw new FormatException($"Primitive keyword was expected. But got keyword of \"{_jackTokenizer.GetKeyword()}\".");
            }

            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
            EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
            );
            _jackTokenizer.Advance();
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
            _jackTokenizer.GetIdentifier()
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        while (tempTokenRef.type is TokenType.IDENTIFIER)
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
            _jackTokenizer.GetIdentifier()
            );
            _jackTokenizer.Advance();

            tempTokenRef = _jackTokenizer.CurrentToken;
            if (tempTokenRef.type == TokenType.SYMBOL && _jackTokenizer.GetSymbol() is ",")
            {
                WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                _jackTokenizer.GetSymbol()
                );
                _jackTokenizer.Advance();
                tempTokenRef = _jackTokenizer.CurrentToken;
            }
        }

        if (tempTokenRef.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not ";")
        {
            throw new FormatException($"Token type of \"{TokenType.SYMBOL}\" with the value of \";\" was expected. But got \"{tempTokenRef.type}\", with the type of \"{tempTokenRef.rawValue}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
            );
        }

        Write("</classVarDec>");
    }
    private void CompileSubroutine()
    {
        Write("<subroutineDec>");
        (TokenType type, string rawValue) tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not TokenType.KEYWORD || _jackTokenizer.GetKeyword() is not (Keyword.CONSTRUCTOR or Keyword.FUNCTION or Keyword.METHOD))
        {
            throw new FormatException($"Token type of \"{TokenType.KEYWORD}\" with the value of \"{Keyword.CONSTRUCTOR}\", \"{Keyword.FUNCTION}\", or \"{Keyword.METHOD}\" was expected. But got token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
            EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not (TokenType.KEYWORD or TokenType.IDENTIFIER))
        {
            throw new FormatException($"Token type of \"{TokenType.KEYWORD}\" or \"{TokenType.IDENTIFIER}\" was expected. But got token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
        }
        else if (tempTokenRef.type is TokenType.KEYWORD)
        {
            if (_jackTokenizer.GetKeyword() is not Keyword.VOID && IsPrimitiveType(_jackTokenizer.GetKeyword()) is false)
            {
                throw new FormatException($"Primitive keyword or \"{Keyword.VOID}\" was expected. But got keyword of \"{_jackTokenizer.GetKeyword()}\".");
            }

            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
            EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
            );
            _jackTokenizer.Advance();
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
            _jackTokenizer.GetIdentifier()
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not TokenType.IDENTIFIER)
        {
            throw new FormatException($"Token type of \"{TokenType.IDENTIFIER}\" was expected. But got token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
            _jackTokenizer.GetIdentifier()
            );
            _jackTokenizer.Advance();
        }

        tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not "(")
        {
            throw new FormatException($"Token type of \"{TokenType.SYMBOL}\" with the value of \"(\" was expected. But got token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
        }
        else
        {
            WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
            );
            _jackTokenizer.Advance();

            CompileParameterList();

            //tempTokenRef = _jackTokenizer.CurrentToken;

            if (tempTokenRef.type is TokenType.SYMBOL && _jackTokenizer.GetSymbol() is ")")
            {
                WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                _jackTokenizer.GetSymbol()
                );
                _jackTokenizer.Advance();
            }
            else
            {
                throw new FormatException($"Expected token type of \"{TokenType.SYMBOL}\" with the value of \")\". But got \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
            }

            if (tempTokenRef.type is not TokenType.SYMBOL)
            {
                throw new FormatException($"Expected token type of \"{TokenType.SYMBOL}\". But got \"{tempTokenRef.type}\".");
            }
            else
            {
                if (_jackTokenizer.GetSymbol() is not "{")
                {
                    throw new FormatException($"Expected symbol of \"{{\". But got \"{tempTokenRef.rawValue}\".");
                }

                CompileSubroutineBody();
            }
        }

        Write("</subroutineDec>");
    }
    private void CompileParameterList()
    {
        Write("<parameterList>");

        (TokenType type, string rawValue) tempTokenRef = _jackTokenizer.CurrentToken;
        while (tempTokenRef.type is TokenType.KEYWORD or TokenType.IDENTIFIER)
        {
            if (tempTokenRef.type is TokenType.KEYWORD)
            {
                if (IsPrimitiveType(_jackTokenizer.GetKeyword()) is false)
                {
                    throw new FormatException($"A primitive is expected. But got \"{_jackTokenizer.GetKeyword()}\".");
                }

                WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
                EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
                );
            }
            else
            {
                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
                    _jackTokenizer.GetIdentifier()
                );
            }

            _jackTokenizer.Advance();

            tempTokenRef = _jackTokenizer.CurrentToken;

            if (tempTokenRef.type is TokenType.SYMBOL && _jackTokenizer.GetSymbol() is ",")
            {
                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                    _jackTokenizer.GetSymbol()
                );
                _jackTokenizer.Advance();
                tempTokenRef = _jackTokenizer.CurrentToken;
            }
        }

        Write("</parameterList>");
    }
    private void CompileSubroutineBody()
    {
        Write("<subroutineBody>");

        (TokenType tokenType, string rawValue) tempTokenRef = _jackTokenizer.CurrentToken;
        if (tempTokenRef.tokenType is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not "{")
        {
            throw new FormatException($"Expected token type of \"{TokenType.SYMBOL}\", with the value of \"{{\". But got token type of \"{tempTokenRef.tokenType}\", with the value of \"{tempTokenRef.rawValue}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
        );
        _jackTokenizer.Advance();

        tempTokenRef = _jackTokenizer.CurrentToken;
        while (tempTokenRef.tokenType is TokenType.KEYWORD && _jackTokenizer.GetKeyword() is Keyword.VAR)
        {
            CompileVarDec();
            tempTokenRef = _jackTokenizer.CurrentToken;
        }

        if (tempTokenRef.tokenType is TokenType.KEYWORD && IsStatementKeyword(_jackTokenizer.GetKeyword()))
        {
            CompileStatements();
            tempTokenRef = _jackTokenizer.CurrentToken;
        }

        if (tempTokenRef.tokenType is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not "}")
        {
            throw new FormatException($"Expected token type of \"{TokenType.SYMBOL}\", with the value of \"}}\". But got token type of \"{tempTokenRef.tokenType}\", with the value of \"{tempTokenRef.rawValue}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
        );
        _jackTokenizer.Advance();

        Write("</subroutineBody>");
    }
    private void CompileVarDec()
    {
        Write("<varDec>");

        (TokenType type, string rawValue) token = _jackTokenizer.CurrentToken;
        if (token.type is not TokenType.KEYWORD || _jackTokenizer.GetKeyword() is not Keyword.VAR)
        {
            throw new FormatException($"The token type of \"{TokenType.KEYWORD}\" with the value of \"{Keyword.VAR}\" is expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
            EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
        );

        _jackTokenizer.Advance();

        token = _jackTokenizer.CurrentToken;
        if (token.type is TokenType.KEYWORD && IsPrimitiveType(_jackTokenizer.GetKeyword()))
        {
            WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
                EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
            );
        }
        else if (token.type is TokenType.IDENTIFIER)
        {
            WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
                _jackTokenizer.GetIdentifier()
            );
        }
        else
        {
            throw new FormatException($"Token type of \"{TokenType.KEYWORD}\" with the value of \"{Keyword.INT}\", or \"{Keyword.CHAR}\", or \"{Keyword.BOOLEAN}\" was expected. Or token type of \"{TokenType.IDENTIFIER}\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
        }

        _jackTokenizer.Advance();
        token = _jackTokenizer.CurrentToken;

        while (token.type is TokenType.IDENTIFIER)
        {
            WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
                _jackTokenizer.GetIdentifier()
            );

            _jackTokenizer.Advance();
            token = _jackTokenizer.CurrentToken;

            if (token.type is TokenType.SYMBOL && _jackTokenizer.GetSymbol() is ",")
            {
                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                    _jackTokenizer.GetSymbol()
                );
                _jackTokenizer.Advance();
                token = _jackTokenizer.CurrentToken;
            }
        }

        if (token.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not ";")
        {
            throw new FormatException($"Token type of the \"{TokenType.SYMBOL}\" with the value of \";\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
        );
        _jackTokenizer.Advance();

        Write("</varDec>");
    }
    private void CompileStatements()
    {
        Write("<statements>");

        (TokenType type, string rawValue) token = _jackTokenizer.CurrentToken;
        while (token.type is TokenType.KEYWORD && IsStatementKeyword(_jackTokenizer.GetKeyword()))
        {
            switch (_jackTokenizer.GetKeyword())
            {
                case Keyword.LET:
                    CompileLet();
                    break;
                case Keyword.IF:
                    CompileIf();
                    break;
                case Keyword.WHILE:
                    CompileWhile();
                    break;
                case Keyword.DO:
                    CompileDo();
                    break;
                case Keyword.RETURN:
                    CompileReturn();
                    break;
                default:
                    throw new FormatException($"One of these keywords were expected: \"{Keyword.LET}\", \"{Keyword.IF}\", \"{Keyword.WHILE}\", \"{Keyword.DO}\", or \"{Keyword.RETURN}\" But got this keyword: \"{_jackTokenizer.GetKeyword()}\".");
            }

            token = _jackTokenizer.CurrentToken;
        }

        Write("</statements>");
    }
    private void CompileLet()
    {
        Write("<letStatement>");

        (TokenType type, string rawValue) token = _jackTokenizer.CurrentToken;
        if (token.type is not TokenType.KEYWORD || _jackTokenizer.GetKeyword() is not Keyword.LET)
        {
            throw new FormatException($"Token type of \"{TokenType.KEYWORD}\" with the value of \"{Keyword.LET}\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(token.type),
            EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
        );
        _jackTokenizer.Advance();

        token = _jackTokenizer.CurrentToken;
        if (token.type is not TokenType.IDENTIFIER)
        {
            throw new FormatException($"Token type of \"{TokenType.IDENTIFIER}\" was expected. But got token type of \"{token.type}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
            _jackTokenizer.GetIdentifier()
        );
        _jackTokenizer.Advance();

        token = _jackTokenizer.CurrentToken;
        if (token.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not ("[" or "="))
        {
            throw new FormatException($"Token type of \"{TokenType.SYMBOL}\", with the value of \"[\" or \"=\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
        );

        _jackTokenizer.Advance();

        CompileExpression();

        token = _jackTokenizer.CurrentToken;

        if (token.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not ("]" or ";"))
        {
            throw new FormatException($"Token type of \"{TokenType.SYMBOL}\", with the value of \"]\" or \";\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
        }
        else if (_jackTokenizer.GetSymbol() is "]")
        {
            WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                _jackTokenizer.GetSymbol()
            );

            _jackTokenizer.Advance();

            token = _jackTokenizer.CurrentToken;

            if (token.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not "=")
            {
                throw new FormatException($"Token type of \"{TokenType.SYMBOL}\" with the value of \"=\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
            }

            WriteNode(
                EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                _jackTokenizer.GetSymbol()
            );

            _jackTokenizer.Advance();

            CompileExpression();
        }

        token = _jackTokenizer.CurrentToken;

        if (token.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not ";")
        {
            throw new FormatException($"Token type of \"{TokenType.SYMBOL}\" with the value of \";\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
        }

        WriteNode(
            EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
            _jackTokenizer.GetSymbol()
        );

        _jackTokenizer.Advance();

        Write("</letStatement>");
    }
    private void CompileIf()
    {
    }
    private void CompileWhile()
    {
    }
    private void CompileDo()
    {
    }
    private void CompileReturn()
    {
    }
    private void CompileExpression()
    {
        Write("<expression>");
        
        bool isContinue = true;

        while (isContinue)
        {
            CompileTerm();

            (TokenType type, string rawValue) token = _jackTokenizer.CurrentToken;

            if (token.type is TokenType.SYMBOL && IsValidOperator(_jackTokenizer.GetSymbol()))
            {
                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                    _jackTokenizer.GetSymbol()
                );

                _jackTokenizer.Advance();
            }
            else
            {
                isContinue = false;
            }

        }

        Write("</expression>");
    }
    private void CompileTerm()
    {
        Write("<term>");

        (TokenType type, string rawValue) token = _jackTokenizer.CurrentToken;
        switch (token.type)
        {
            case TokenType.INT_CONST:
                WriteNode("integerConstant", _jackTokenizer.GetUInt15Constant().ToString());
                _jackTokenizer.Advance();
                break;
            case TokenType.STRING_CONST:
                WriteNode("stringConstant", _jackTokenizer.GetString());
                _jackTokenizer.Advance();
                break;
            case TokenType.KEYWORD when IsKeywordConstant(_jackTokenizer.GetKeyword()):
                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(token.type),
                    EnumToString<Keyword>.ConvertToLower(_jackTokenizer.GetKeyword())
                );
                _jackTokenizer.Advance();
                break;
            case TokenType.IDENTIFIER when !(_jackTokenizer.Peek().type is TokenType.SYMBOL && _jackTokenizer.Peek().value is "." or "("):
                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
                    _jackTokenizer.GetIdentifier()
                );

                _jackTokenizer.Advance();

                token = _jackTokenizer.CurrentToken;

                if (token.type is TokenType.SYMBOL && _jackTokenizer.GetSymbol() is "[")
                {
                    WriteNode(
                        EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                        _jackTokenizer.GetSymbol()
                    );

                    _jackTokenizer.Advance();

                    CompileExpression();

                    token = _jackTokenizer.CurrentToken;

                    if (token.type is not TokenType.SYMBOL && _jackTokenizer.GetSymbol() is not "]")
                    {
                        throw new FormatException($"Token type of \"{TokenType.SYMBOL}\" with the value of \"]\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
                    }

                    WriteNode(
                        EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                        _jackTokenizer.GetSymbol()
                    );

                    _jackTokenizer.Advance();
                }
                break;
            case TokenType.SYMBOL when _jackTokenizer.GetSymbol() is "(" or "-" or "~":
                if (_jackTokenizer.GetSymbol() is "(")
                {
                    WriteNode(
                        EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                        _jackTokenizer.GetSymbol()
                    );

                    _jackTokenizer.Advance();

                    CompileExpression();

                    token = _jackTokenizer.CurrentToken;

                    if (token.type is not TokenType.SYMBOL && _jackTokenizer.GetSymbol() is not ")")
                    {
                        throw new FormatException($"Token type of \"{TokenType.SYMBOL}\" with the value of \")\" was expected. But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
                    }

                    WriteNode(
                        EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                        _jackTokenizer.GetSymbol()
                    );

                    _jackTokenizer.Advance();
                }
                else
                {
                    WriteNode(
                        EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                        _jackTokenizer.GetSymbol()
                    );
                    
                    _jackTokenizer.Advance();

                    CompileTerm();
                }
                break;
            case TokenType.IDENTIFIER when _jackTokenizer.Peek().type is TokenType.SYMBOL && _jackTokenizer.Peek().value is "." or "(":
                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
                    _jackTokenizer.GetIdentifier()
                );

                _jackTokenizer.Advance();

                if (_jackTokenizer.GetSymbol() is ".")
                {
                    WriteNode(
                        EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                        _jackTokenizer.GetSymbol()
                    );

                    _jackTokenizer.Advance();

                    token = _jackTokenizer.CurrentToken;

                    if (token.type is not TokenType.IDENTIFIER)
                    {
                        throw new FormatException($"Token type of \"{TokenType.IDENTIFIER}\" was expected. But got token type of \"{token.type}\".");
                    }

                    WriteNode(
                        EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
                        _jackTokenizer.GetIdentifier()
                    );

                    _jackTokenizer.Advance();
                }

                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                    _jackTokenizer.GetSymbol()
                );

                _jackTokenizer.Advance();

                CompileExpressionList();

                token = _jackTokenizer.CurrentToken;

                if (token.type is not TokenType.SYMBOL || _jackTokenizer.GetSymbol() is not ")")
                {
                    throw new FormatException($"Expected token type of \"{TokenType.SYMBOL}\" with the value of \")\". But got token type of \"{token.type}\" with the value of \"{token.rawValue}\".");
                }

                WriteNode(
                    EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
                    _jackTokenizer.GetSymbol()
                );

                _jackTokenizer.Advance();

                break;
            default:
                throw new FormatException($"Unexpected token type for the term. The token type was \"{token.type}\".");
        }

        Write("</term>");
    }
    private int CompileExpressionList()
    {
        throw new NotImplementedException();
    }
    private void Write(string message)
    {
        _outputWriter.WriteLine(message);
    }
    private void WriteNode(string nodeType, string value)
    {
        _outputWriter.WriteLine($"<{nodeType}> {value} </{nodeType}>");
    }
    private static bool IsPrimitiveType(Keyword keyword)
    {
        return keyword is Keyword.INT or Keyword.BOOLEAN or Keyword.CHAR;
    }
    private static bool IsStatementKeyword(Keyword keyword)
    {
        return keyword is Keyword.LET or Keyword.IF or Keyword.WHILE or Keyword.DO or Keyword.RETURN;
    }
    private static bool IsKeywordConstant(Keyword keyword)
    {
        return keyword is Keyword.TRUE or Keyword.FALSE or Keyword.NULL or Keyword.THIS;
    }
    private static bool IsValidOperator(string op)
    {
        return op is "+" or "-" or "*" or "/" or "&amp;" or "|" or "&lt;" or "&gt;" or "=";
    }
    public void Dispose()
    {
        if (_isDisposed is false)
        {
            _outputWriter.Dispose();
            _isDisposed = true;
        }
    }
}