namespace JackCompiler;

class CompilationEngine
{
	private readonly JackTokenizer _jackTokenizer;
	private readonly string _outputFileName;
	public CompilationEngine(JackTokenizer jackTokenizer, string outputFileName)
	{
		_jackTokenizer = jackTokenizer;
		_outputFileName = outputFileName;
		CompileClass();
	}
	public void CompileClass() { }
	public void CompileClassVarDec() { }
	public void CompileSubroutine() { }
	public void CompileParameterList() { }
	public void CompileSubroutineBody() { }
	public void CompileVarDec() { }
	public void CompileStatements() { }
	public void CompileLet() { }
	public void CompileIf() { }
	public void CompileWhile() { }
	public void CompileDo() { }
	public void CompileReturn() { }
	public void CompileExpression() { }
	public void CompileTerm() { }
	public int CompileExpressionList()
	{
		throw new NotImplementedException();
	}
}