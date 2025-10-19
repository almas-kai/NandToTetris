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

		(TokenType type, string rawValue) tempTokenRef = _jackTokenizer.CurrentToken;
		if (tempTokenRef.type != TokenType.KEYWORD || tempTokenRef.rawValue != EnumToString<Keyword>.ConvertToLower(Keyword.CLASS))
		{
			throw new FormatException($"The token type of \"{TokenType.KEYWORD}\" with the value of \"{Keyword.CLASS}\" was expected. But got the token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
		}
		else
		{
			_WriteNode(
				EnumToString<TokenType>.ConvertToLower(TokenType.KEYWORD),
				EnumToString<Keyword>.ConvertToLower(
					_jackTokenizer.GetKeyword()
				)
			);
			_jackTokenizer.Advance();
		}

		tempTokenRef = _jackTokenizer.CurrentToken;
		if (tempTokenRef.type != TokenType.IDENTIFIER)
		{
			throw new FormatException($"The token type of \"{TokenType.IDENTIFIER}\" was expected. But got: \"{tempTokenRef.type}\".");
		}
		else
		{
			_WriteNode(
				EnumToString<TokenType>.ConvertToLower(TokenType.IDENTIFIER),
				_jackTokenizer.GetIdentifier()
			);
			_jackTokenizer.Advance();
		}

		tempTokenRef = _jackTokenizer.CurrentToken;
		if (tempTokenRef.type != TokenType.SYMBOL || tempTokenRef.rawValue != "{")
		{
			throw new FormatException($"The token type of \"{TokenType.SYMBOL}\" with the value of \"{{\" was expected. But got the token type of \"{tempTokenRef.type}\" with the value of \"{tempTokenRef.rawValue}\".");
		}
		else
		{
			_WriteNode(
				EnumToString<TokenType>.ConvertToLower(TokenType.SYMBOL),
				_jackTokenizer.GetSymbol()
			);
			_jackTokenizer.Advance();
		}

		_Write("</class>");
	}
	private void CompileClassVarDec()
	{
	}
	private void CompileSubroutine()
	{
	}
	private void CompileParameterList()
	{
	}
	private void CompileSubroutineBody()
	{
	}
	private void CompileVarDec()
	{
	}
	private void CompileStatements()
	{
	}
	private void CompileLet()
	{
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
	}
	private void CompileTerm()
	{
	}
	private int CompileExpressionList()
	{
		throw new NotImplementedException();
	}
	private void _Write(string message)
	{
		_outputWriter.WriteLine(message);
	}
	private void _WriteNode(string nodeType, string value)
	{
		_Write($"<{nodeType}>");
		_Write(value);
		_Write($"</{nodeType}>");
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