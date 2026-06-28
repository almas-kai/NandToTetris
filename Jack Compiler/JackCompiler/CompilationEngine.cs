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

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.CLASS]
        );

        Consume(
            expectedNodeType: TokenType.IDENTIFIER
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_BRACE]
        );

        ConsumeWhile(
            predicate: (TokenType type, string value) => type is TokenType.KEYWORD && Keyword.IsClassVarDec(value),
            action: CompileClassVarDec
        );

        ConsumeWhile(
            predicate: (TokenType type, string value) => type is TokenType.KEYWORD && Keyword.IsSubroutineDec(value),
            action: CompileSubroutine,
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_BRACE],
            isStartWithAdvance: false
        );

        Write("</class>");
    }
    private void CompileClassVarDec()
    {
        Write("<classVarDec>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.FIELD, Keyword.STATIC],
            isStartWithAdvance: false
        );

        ConsumeIdentifierOrKeyword(
            expectedKeywords: [
                Keyword.INT,
                Keyword.BOOLEAN,
                Keyword.CHAR
            ]
        );

        // This part is weird.
        _jackTokenizer.Advance();
        (TokenType type, string value) token = _jackTokenizer.CurrentToken;

        while (token.type is TokenType.IDENTIFIER)
        {
            WriteNode(
                TokenType.IDENTIFIER.ToLowerString(),
                token.value
            );

            _jackTokenizer.Advance();
            token = _jackTokenizer.CurrentToken;

            if (token.type == TokenType.SYMBOL && token.value is Symbol.COMMA)
            {
                WriteNode(
                    token.type.ToLowerString(),
                    token.value
                );

                _jackTokenizer.Advance();
                token = _jackTokenizer.CurrentToken;
            }
        }

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.SEMICOLON],
            isStartWithAdvance: false
        );

        Write("</classVarDec>");
    }
    private void CompileSubroutine()
    {
        Write("<subroutineDec>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [
                Keyword.CONSTRUCTOR,
                Keyword.FUNCTION,
                Keyword.METHOD
            ],
            isStartWithAdvance: false
        );

        ConsumeIdentifierOrKeyword(
            expectedKeywords: [
                Keyword.INT,
                Keyword.BOOLEAN,
                Keyword.CHAR,
                Keyword.VOID
            ]
        );

        Consume(
            expectedNodeType: TokenType.IDENTIFIER
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_PARENTHESIS]
        );

        CompileParameterList();

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_PARENTHESIS],
            isStartWithAdvance: false
        );

        CompileSubroutineBody();

        Write("</subroutineDec>");
    }
    private void CompileParameterList()
    {
        Write("<parameterList>");

        _jackTokenizer.Advance();

        // Again need to rework this loop as well.
        (TokenType type, string value) token = _jackTokenizer.CurrentToken;
        while (token.type is TokenType.KEYWORD or TokenType.IDENTIFIER)
        {
            ConsumeIdentifierOrKeyword(
                expectedKeywords: [
                    Keyword.INT,
                    Keyword.BOOLEAN,
                    Keyword.CHAR
                ],
                isStartWithAdvance: false
            );

            _jackTokenizer.Advance();

            token = _jackTokenizer.CurrentToken;

            if (token.type is TokenType.SYMBOL && token.value is Symbol.COMMA)
            {
                // You could give out token itself here.
                WriteNode(
                    token.type.ToLowerString(),
                    token.value
                );
                _jackTokenizer.Advance();
                token = _jackTokenizer.CurrentToken;
            }
        }

        Write("</parameterList>");
    }
    private void CompileSubroutineBody()
    {
        Write("<subroutineBody>");

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_BRACE]
        );

        ConsumeWhile(
            predicate: (TokenType type, string value) => type is TokenType.KEYWORD && value is Keyword.VAR,
            action: CompileVarDec
        );

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;

        if (token.type is TokenType.KEYWORD && Keyword.IsStatementKeyword(token.value))
        {
            CompileStatements();
        }

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_BRACE],
            isStartWithAdvance: false
        );

        Write("</subroutineBody>");
    }
    private void CompileVarDec()
    {
        Write("<varDec>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.VAR],
            isStartWithAdvance: false
        );

        ConsumeIdentifierOrKeyword(
            expectedKeywords: [
                Keyword.INT,
                Keyword.BOOLEAN,
                Keyword.CHAR
            ]
        );

        _jackTokenizer.Advance();
        (TokenType type, string value) token = _jackTokenizer.CurrentToken;
        // Again while loop that i don't see how to extract.
        while (token.type is TokenType.IDENTIFIER)
        {
            WriteNode(
                token.type.ToLowerString(),
                token.value
            );

            _jackTokenizer.Advance();
            token = _jackTokenizer.CurrentToken;

            if (token.type is TokenType.SYMBOL && token.value is Symbol.COMMA)
            {
                WriteNode(
                    token.type.ToLowerString(),
                    token.value
                );
                _jackTokenizer.Advance();
                token = _jackTokenizer.CurrentToken;
            }
        }

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.SEMICOLON],
            isStartWithAdvance: false
        );

        Write("</varDec>");
    }
    private void CompileStatements()
    {
        Write("<statements>");
        // Can we move advance here?

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;
        while (token.type is TokenType.KEYWORD && Keyword.IsStatementKeyword(token.value))
        {
            switch (token.value)
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
                    throw new FormatExceptionBuilder()
                        .AddUnexpected(token.type, token.value)
                        .AddExpected(
                            TokenType.KEYWORD,
                            [
                                Keyword.LET,
                                Keyword.IF,
                                Keyword.WHILE,
                                Keyword.DO,
                                Keyword.RETURN
                            ])
                        .Build();
            }

            token = _jackTokenizer.CurrentToken;
        }

        Write("</statements>");
    }
    private void CompileLet()
    {
        Write("<letStatement>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.LET],
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.IDENTIFIER
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [
                Symbol.OPENING_BRACKET,
                Symbol.EQUAL
            ]
        );

        _jackTokenizer.Advance();

        CompileExpression();

        // Improve!

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;

        if (token.type is not TokenType.SYMBOL || token.value is not (Symbol.CLOSING_BRACKET or Symbol.SEMICOLON))
        {
            throw new FormatExceptionBuilder()
                .AddUnexpected(token.type, token.value)
                .AddExpected(TokenType.SYMBOL, Symbol.CLOSING_BRACKET, Symbol.SEMICOLON)
                .Build();
        }
        else if (token.value is Symbol.CLOSING_BRACKET)
        {
            WriteNode(
                TokenType.SYMBOL.ToLowerString(),
                token.value
            );

            Consume(
                expectedNodeType: TokenType.SYMBOL,
                expectedValues: [Symbol.EQUAL]
            );

            _jackTokenizer.Advance();

            CompileExpression();
        }

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.SEMICOLON],
            isStartWithAdvance: false
        );

        _jackTokenizer.Advance();

        Write("</letStatement>");
    }
    private void CompileIf()
    {
        Write("<ifStatement>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.IF],
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_PARENTHESIS]
        );

        _jackTokenizer.Advance();

        CompileExpression();

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_PARENTHESIS],
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_BRACE]
        );

        _jackTokenizer.Advance();

        CompileStatements();

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_BRACE],
            isStartWithAdvance: false
        );

        _jackTokenizer.Advance();

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;

        if (token.type is TokenType.KEYWORD && token.value is Keyword.ELSE)
        {
            WriteNode(
                TokenType.KEYWORD.ToLowerString(),
                Keyword.ELSE
            );

            Consume(
                expectedNodeType: TokenType.SYMBOL,
                expectedValues: [Symbol.OPENING_BRACE]
            );

            _jackTokenizer.Advance();

            CompileStatements();

            Consume(
                expectedNodeType: TokenType.SYMBOL,
                expectedValues: ["}"],
                isStartWithAdvance: false
            );

            _jackTokenizer.Advance();
        }

        Write("</ifStatement>");
    }
    private void CompileWhile()
    {
        Write("<whileStatement>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.WHILE],
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_PARENTHESIS]
        );

        _jackTokenizer.Advance();

        CompileExpression();

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_PARENTHESIS],
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_BRACE]
        );

        _jackTokenizer.Advance();

        CompileStatements();

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_BRACE],
            isStartWithAdvance: false
        );

        _jackTokenizer.Advance();

        Write("</whileStatement>");
    }
    private void CompileDo()
    {
        Write("<doStatement>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.DO],
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.IDENTIFIER
        );

        if (_jackTokenizer.Peek().value is Symbol.DOT)
        {
            Consume(
                expectedNodeType: TokenType.SYMBOL,
                expectedValues: [Symbol.DOT]
            );

            Consume(
                expectedNodeType: TokenType.IDENTIFIER
            );
        }

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.OPENING_PARENTHESIS]
        );

        _jackTokenizer.Advance();

        CompileExpressionList();

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.CLOSING_PARENTHESIS],
            isStartWithAdvance: false
        );

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.SEMICOLON]
        );

        _jackTokenizer.Advance();

        Write("</doStatement>");
    }
    private void CompileReturn()
    {
        Write("<returnStatement>");

        Consume(
            expectedNodeType: TokenType.KEYWORD,
            expectedValues: [Keyword.RETURN],
            isStartWithAdvance: false
        );

        _jackTokenizer.Advance();

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;

        if (token.type is not TokenType.SYMBOL || token.value is not Symbol.SEMICOLON)
        {
            CompileExpression();
        }

        Consume(
            expectedNodeType: TokenType.SYMBOL,
            expectedValues: [Symbol.SEMICOLON],
            isStartWithAdvance: false
        );

        _jackTokenizer.Advance();

        Write("</returnStatement>");
    }
    private void CompileExpression()
    {
        // Should compile expression call the advance for itself?
        Write("<expression>");
        
        bool isContinue = true;

        while (isContinue)
        {
            CompileTerm();

            (TokenType type, string value) token = _jackTokenizer.CurrentToken;

            // Remove GetSymbol, rework the tokenizer.
            if (token.type is TokenType.SYMBOL && Symbol.IsValidOperator(token.value))
            {
                WriteNode(
                    TokenType.SYMBOL.ToLowerString(),
                    token.value
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

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;
        switch (token.type)
        {
            case TokenType.INT_CONST:
                WriteNode("integerConstant", token.value);
                _jackTokenizer.Advance();
                break;
            case TokenType.STRING_CONST:
                WriteNode("stringConstant", token.value);
                _jackTokenizer.Advance();
                break;
            case TokenType.KEYWORD when Keyword.IsKeywordConstant(token.value):
                WriteNode(
                    token.type.ToLowerString(),
                    token.value
                );
                _jackTokenizer.Advance();
                break;
            case TokenType.IDENTIFIER when !(_jackTokenizer.Peek().type is TokenType.SYMBOL && _jackTokenizer.Peek().value is Symbol.DOT or Symbol.OPENING_PARENTHESIS):
                WriteNode(
                    TokenType.IDENTIFIER.ToLowerString(),
                    token.value
                );

                _jackTokenizer.Advance();

                token = _jackTokenizer.CurrentToken;

                if (token.type is TokenType.SYMBOL && token.value is Symbol.OPENING_BRACKET)
                {
                    WriteNode(
                        TokenType.SYMBOL.ToLowerString(),
                        token.value
                    );

                    _jackTokenizer.Advance();

                    CompileExpression();

                    Consume(
                        expectedNodeType: TokenType.SYMBOL,
                        expectedValues: [Symbol.CLOSING_BRACKET],
                        isStartWithAdvance: false
                    );

                    _jackTokenizer.Advance();
                }
                break;
            case TokenType.SYMBOL when token.value is Symbol.OPENING_PARENTHESIS or Symbol.MINUS or Symbol.TILDE:
                if (token.value is Symbol.OPENING_PARENTHESIS)
                {
                    WriteNode(
                        TokenType.SYMBOL.ToLowerString(),
                        token.value
                    );

                    _jackTokenizer.Advance();

                    CompileExpression();

                    Consume(
                        expectedNodeType: TokenType.SYMBOL,
                        expectedValues: [Symbol.CLOSING_PARENTHESIS],
                        isStartWithAdvance: false
                    );

                    _jackTokenizer.Advance();
                }
                else
                {
                    WriteNode(
                        TokenType.SYMBOL.ToLowerString(),
                        token.value
                    );
                    
                    _jackTokenizer.Advance();

                    CompileTerm();
                }
                break;
            case TokenType.IDENTIFIER when _jackTokenizer.Peek().type is TokenType.SYMBOL && _jackTokenizer.Peek().value is Symbol.DOT or Symbol.OPENING_PARENTHESIS:
                WriteNode(
                    TokenType.IDENTIFIER.ToLowerString(),
                    token.value
                );

                _jackTokenizer.Advance();

                token = _jackTokenizer.CurrentToken;

                if (token.value is Symbol.DOT)
                {
                    WriteNode(
                        TokenType.SYMBOL.ToLowerString(),
                        token.value
                    );

                    Consume(
                        expectedNodeType: TokenType.IDENTIFIER
                    );

                    _jackTokenizer.Advance();
                }

                token = _jackTokenizer.CurrentToken;

                WriteNode(
                    TokenType.SYMBOL.ToLowerString(),
                    token.value
                );

                _jackTokenizer.Advance();

                CompileExpressionList();

                Consume(
                    expectedNodeType: TokenType.SYMBOL,
                    expectedValues: [Symbol.CLOSING_PARENTHESIS],
                    isStartWithAdvance: false
                );

                _jackTokenizer.Advance();

                break;
            default:
                throw new FormatExceptionBuilder()
                    .AddUnexpected(token.type)
                    .Build();
        }

        Write("</term>");
    }

    private int CompileExpressionList()
    {
        int count = 0;
        Write("<expressionList>");

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;
        while (token.type is not TokenType.SYMBOL || token.value is not Symbol.CLOSING_PARENTHESIS)
        {
            count ++;
            CompileExpression();

            token = _jackTokenizer.CurrentToken;
            if (token.type is TokenType.SYMBOL && token.value is Symbol.COMMA)
            {
                WriteNode(
                    TokenType.SYMBOL.ToLowerString(),
                    token.value
                );

                _jackTokenizer.Advance();

                token = _jackTokenizer.CurrentToken;
            }
        }

        Write("</expressionList>");
        return count;
    }

    private void Write(string message)
    {
        _outputWriter.WriteLine(message);
    }

    private void WriteNode(string nodeType, string value)
    {
        _outputWriter.WriteLine($"<{nodeType}> {value} </{nodeType}>");
    }

    private void WriteNode<TNode>(TNode nodeType, string value)
        where TNode : struct, Enum
    {
        _outputWriter.WriteLine($"<{nodeType.ToLowerString()}> {value} </{nodeType.ToLowerString()}>");
    }

    private void Consume(
        TokenType expectedNodeType,
        string[]? expectedValues = null,
        bool isStartWithAdvance = true
    )
    {
        if (isStartWithAdvance)
        {
            _jackTokenizer.Advance();
        }

        expectedValues ??= Array.Empty<string>();

        var token = _jackTokenizer.CurrentToken;

        if (expectedNodeType != token.Type || (expectedValues.Length > 0 && !expectedValues.Contains(token.RawValue)))
        {
            throw new FormatExceptionBuilder()
                .AddUnexpected(token.Type, token.RawValue)
                .AddExpected(expectedNodeType, expectedValues)
                .Build();
        }

        WriteNode(token.Type, token.RawValue);
    }

    private void ConsumeWhile(
        Func<TokenType, string, bool> predicate,
        Action action,
        bool isStartWithAdvance = true
    )
    {
        if (isStartWithAdvance)
        {
            _jackTokenizer.Advance();
        }

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;

        while (predicate(token.type, token.value))
        {
            action();
            _jackTokenizer.Advance();
            token = _jackTokenizer.CurrentToken;
        }
    }

    private void ConsumeIdentifierOrKeyword(
        string[] expectedKeywords,
        bool isStartWithAdvance = true
    )
    {
        if (isStartWithAdvance)
        {
            _jackTokenizer.Advance();
        }

        (TokenType type, string value) token = _jackTokenizer.CurrentToken;

        if (token.type is not (TokenType.KEYWORD or TokenType.IDENTIFIER))
        {
            throw new FormatExceptionBuilder()
                .AddUnexpected(token.type, token.value)
                .AddExpected(TokenType.KEYWORD)
                .AddExpected(TokenType.IDENTIFIER)
                .Build();
        }
        else if (token.type is TokenType.KEYWORD)
        {
            if (!expectedKeywords.Contains(token.value))
            {
                throw new FormatExceptionBuilder()
                    .AddUnexpected(token.type, token.value)
                    .AddExpected(TokenType.KEYWORD, [
                        Keyword.INT,
                        Keyword.BOOLEAN,
                        Keyword.CHAR
                    ])
                    .Build();
            }
        }

        WriteNode(token.type, token.value);
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