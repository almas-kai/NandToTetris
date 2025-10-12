using JackCompiler.Utilities;
using JackCompiler.Utilities.ConstantsAndEnums;

namespace JackCompiler;

internal class CompilationEngine : IDisposable
{
	private readonly JackTokenizer _jackTokenizer;
	private readonly string _outputFileName;
	private StreamWriter _outputWriter;
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
		_Write("<class>");

		_jackTokenizer.Advance();
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.CLASS)
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else if (keyword is Keyword.STATIC || keyword is Keyword.FIELD)
					{
						CompileClassVarDec();
					}
					else if (keyword is Keyword.METHOD || keyword is Keyword.FUNCTION || keyword is Keyword.CONSTRUCTOR)
					{
						CompileSubroutine();
					}
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					_Write($"<symbol> {symbol} </symbol>");
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					_Write($"<identifier> {identifier} </identifier>");
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}

			_jackTokenizer.Advance();
		}

		_Write("</class>");
	}
	private void CompileClassVarDec()
	{
		bool isDone = false;
		_Write("<classVarDec>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.STATIC || keyword is Keyword.FIELD || _IsPrimitiveType(keyword))
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else
					{
						throw new FormatException($"Unrecognized keyword: \"{keyword}\".");
					}
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					_Write($"<identifier> {identifier} </identifier>");
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "," || symbol == ";")
					{
						_Write($"<symbol> {symbol} </symbol>");
						if (symbol == ";")
						{
							isDone = true;
						}
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: {tokenType}.");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</classVarDec>");
	}
	private void CompileSubroutine()
	{
		bool isDone = false;
		_Write("<subroutineDec>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.FUNCTION || keyword is Keyword.METHOD || keyword is Keyword.CONSTRUCTOR || keyword is Keyword.VOID || _IsPrimitiveType(keyword))
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					_Write($"<identifier> {identifier} </identifier>");
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "(")
					{
						_Write($"<symbol> {symbol} </symbol>");
						_jackTokenizer.Advance();
						CompileParameterList();
						string afterSymbol = _jackTokenizer.GetSymbol();
						if (afterSymbol == ")")
						{
							_Write($"<symbol> {afterSymbol} </symbol>");
							_jackTokenizer.Advance();
							CompileSubroutineBody();
							isDone = true;
						}
						else
						{
							throw new FormatException($"Unrecognized symbol type: \"{afterSymbol}\".");
						}
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</subroutineDec>");
	}
	private void CompileParameterList()
	{
		bool isDone = false;
		_Write("<parameterList>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (_IsPrimitiveType(keyword))
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					_Write($"<identifier> {identifier} </identifier>");
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == ",")
					{
						_Write($"<symbol> {symbol} </symbol>");
					}
					else if (symbol == ")")
					{
						isDone = true;
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</parameterList>");
	}
	private void CompileSubroutineBody()
	{
		bool isDone = false;
		_Write("<subroutineBody>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.VAR)
					{
						CompileVarDec();
					}
					else if (keyword is Keyword.LET || keyword is Keyword.IF || keyword is Keyword.WHILE || keyword is Keyword.DO || keyword is Keyword.RETURN)
					{
						CompileStatements();
						string afterSymbol = _jackTokenizer.GetSymbol();
						if (afterSymbol == "}")
						{
							_Write($"<symbol> {afterSymbol} </symbol>");
							isDone = true;
						}
						else
						{
							throw new FormatException($"Unrecognized after symbol type: \"{afterSymbol}\".");
						}
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "{" || symbol == "}")
					{
						_Write($"<symbol> {symbol} </symbol>");
						if (symbol == "}")
						{
							isDone = true;
						}
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</subroutineBody>");
	}
	private void CompileVarDec()
	{
		bool isDone = false;
		_Write("<varDec>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.VAR || _IsPrimitiveType(keyword))
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					_Write($"<identifier> {identifier} </identifier>");
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "," || symbol == ";")
					{
						_Write($"<symbol> {symbol} </symbol>");
						if (symbol == ";")
						{
							isDone = true;
						}
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</varDec>");
	}
	private void CompileStatements()
	{
		bool isDone = false;
		_Write("<statements>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.LET)
					{
						CompileLet();
					}
					else if (keyword is Keyword.IF)
					{
						CompileIf();
					}
					else if (keyword is Keyword.WHILE)
					{
						CompileWhile();
					}
					else if (keyword is Keyword.DO)
					{
						CompileDo();
					}
					else if (keyword is Keyword.RETURN)
					{
						CompileReturn();
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "}")
					{
						isDone = true;
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</statements>");
	}
	private void CompileLet()
	{
		bool isDone = false;
		_Write("<letStatement>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.LET)
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					_Write($"<identifier> {identifier} </identifier>");
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "[" || symbol == "=")
					{
						_Write($"<symbol> {symbol} </symbol>");
						CompileExpression();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
						if (symbol == ";")
						{
							isDone = true;
						}
						else if (symbol != "]")
						{
							throw new FormatException($"Unrecognized symbol type: \"{afterSymbol}\".");
						}
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</letStatement>");
	}
	private void CompileIf()
	{
		bool isDone = false;
		_Write("<ifStatement>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					string keywordAsString = KeywordToString.ConvertToLower(keyword);
					if (keyword is Keyword.IF)
					{
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else if (keyword is Keyword.ELSE)
					{
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else
					{
						throw new FormatException($"Unrecognized keyword: \"{keyword}\".");
					}
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "(")
					{
						CompileExpression();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
					}
					else if (symbol == "{")
					{
						_Write($"<symbol> {symbol} </symbol>");
						CompileStatements();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
						(TokenType type, string value) token = _jackTokenizer.Peek();
						if (!(token.type is TokenType.KEYWORD && token.value == "else"))
						{
							isDone = true;
						}
					}
					else
					{
						throw new FormatException($"Unrecognized symbol: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: {tokenType}.");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</ifStatement>");
	}
	private void CompileWhile()
	{
		bool isDone = false;
		_Write("<whileStatement>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword == Keyword.WHILE)
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "(")
					{
						_Write($"<symbol> {symbol} </symbol>");
						CompileExpression();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
					}
					else if (symbol == "{")
					{
						_Write($"<symbol> {symbol} </symbol>");
						CompileStatements();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
						isDone = true;
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</whileStatement>");
	}
	private void CompileDo()
	{
		bool isDone = false;
		_Write("<doStatement>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.DO)
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					_Write($"<identifier> {identifier} </identifier>");
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					_Write($"<symbol> {symbol} </symbol>");
					if (symbol == "(")
					{
						CompileExpressionList();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
					}
					else if (symbol == "." || symbol == ";")
					{
						_Write($"<symbol> {symbol} </symbol>");
						isDone = symbol == ";" ? true : false;
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</doStatement>");
	}
	private void CompileReturn()
	{
		bool isDone = false;
		_Write("<returnStatement>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.RETURN)
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
						(TokenType type, string value) token = _jackTokenizer.Peek();
						if (!(token.type is TokenType.SYMBOL && token.value == ";"))
						{
							CompileExpression();
							string afterSymbol = _jackTokenizer.GetSymbol();
							_Write($"<symbol> {afterSymbol} </symbol>");
							isDone = true;
						}
					}
					else
					{
						throw new FormatException($"Unrecognized keyword type: \"{keyword}\".");
					}
					break;
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == ";")
					{
						_Write($"<symbol> {symbol} </symbol>");
						isDone = true;
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</returnStatement>");
	}
	private void CompileExpression()
	{
		bool isDone = false;
		_Write("<expression>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.SYMBOL:
					string symbol = _jackTokenizer.GetSymbol();
					if (symbol == "+" || symbol == "-" || symbol == "*" || symbol == "/" || symbol == "&" || symbol == "|" || symbol == "<" || symbol == ">" || symbol == "=")
					{
						_Write($"<symbol> {symbol} </symbol>");
					}
					else if (symbol == ";" || symbol == ")" || symbol == "]")
					{
						isDone = true;
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{symbol}\".");
					}
					break;
				default:
					CompileTerm();
					break;
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</expression>");
	}
	private void CompileTerm()
	{
		bool isDone = false;
		_Write("<term>");
		while (_jackTokenizer.HasMoreTokens)
		{
			TokenType tokenType = _jackTokenizer.CurrentToken.Type;
			switch (tokenType)
			{
				case TokenType.INT_CONST:
					int intConst = _jackTokenizer.GetInteger();
					_Write($"<integerConstant> {intConst} </integerConstant>");
					isDone = true;
					break;
				case TokenType.STRING_CONST:
					string strConst = _jackTokenizer.GetString();
					_Write($"<stringConstant> {strConst} </stringConstant>");
					isDone = true;
					break;
				case TokenType.KEYWORD:
					Keyword keyword = _jackTokenizer.GetKeyword();
					if (keyword is Keyword.TRUE || keyword is Keyword.FALSE || keyword is Keyword.NULL || keyword is Keyword.THIS)
					{
						string keywordAsString = KeywordToString.ConvertToLower(keyword);
						_Write($"<keyword> {keywordAsString} </keyword>");
						isDone = true;
					}
					else
					{
						throw new FormatException($"Unrecognized keyword: \"{keyword}\".");
					}
					break;
				case TokenType.IDENTIFIER:
					string identifier = _jackTokenizer.GetIdentifier();
					(TokenType type, string value) token = _jackTokenizer.Peek();
					_Write($"<identifier> {identifier} </identifier>");
					if (token.value == "[")
					{
						_jackTokenizer.Advance();
						_Write($"<symbol> {token.value} </symbol>");
						CompileExpression();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
					}
					else if (token.value == "(")
					{
						_jackTokenizer.Advance();
						_Write($"<symbol> {token.value} </symbol>");
						CompileExpression();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
					}
					else if (token.value == ".")
					{
						_Write($"<symbol> {token.value} </symbol>");
						_jackTokenizer.Advance();
						_jackTokenizer.Advance();
						string secondIdentifier = _jackTokenizer.GetIdentifier();
						_Write($"<identifier> {secondIdentifier} </identifier>");
						_jackTokenizer.Advance();
						string symbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {symbol} </symbol>");
						_jackTokenizer.Advance();
						CompileExpressionList();
						string afterSymbol = _jackTokenizer.GetSymbol();
						_Write($"<symbol> {afterSymbol} </symbol>");
					}
					isDone = true;
					break;
				case TokenType.SYMBOL:
					string unaryOp = _jackTokenizer.GetSymbol();
					if (unaryOp == "-" || unaryOp == "~")
					{
						_Write($"<symbol> {unaryOp} </symbol>");
					}
					else
					{
						throw new FormatException($"Unrecognized symbol type: \"{unaryOp}\".");
					}
					break;
				default:
					throw new FormatException($"Unrecognized token type: \"{tokenType}\".");
			}
			if (isDone)
			{
				break;
			}
			_jackTokenizer.Advance();
		}
		_Write("</term>");
	}
	private int CompileExpressionList()
	{
		throw new NotImplementedException();
	}
	private void _Write(string message)
	{
		_outputWriter.WriteLine(message);
	}
	private bool _IsPrimitiveType(Keyword keyword)
	{
		return keyword is Keyword.INT || keyword is Keyword.BOOLEAN || keyword is Keyword.CHAR;
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