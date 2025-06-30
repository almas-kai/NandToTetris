
namespace VMTranslator;

public class CodeWriter
{
	private int _labelID = 0;
	public string WriteArithmetic(string command)
	{
		switch (command)
		{
			case "add":
				return Add();
			case "sub":
				return Subtract();
			case "neg":
				return Negate();
			case "eq":
				return Equal();
			case "gt":
				return GreaterThan();
			case "lt":
				return LessThan();
			case "and":
				return And();
			case "or":
				return Or();
			case "not":
				return Not();
			default:
				throw new InvalidOperationException($"Code writer error. Inappropriate command type: {command}.");
		}
	}
	public string WritePushPop(CommandType commandType, string segment, int index, string fileName)
	{
		if (commandType is not CommandType.C_PUSH && commandType is not CommandType.C_POP)
		{
			throw new InvalidOperationException($"Code writer error. Inappropriate command type: {commandType}. It has to be push or pop command.");
		}
		if (index < 0)
		{
			throw new ArgumentException($"Code writer error. The index cannot be negative: {index}.");
		}
		switch (segment)
		{
			case "argument" or "local" or "this" or "that":
				return SegmentPopPush(segment, index, commandType);
			case "pointer":
				segment = index == 0 ? "this" : "that";

				string pointerAssembly = "// <- POINTER PUSH/POP BEGIN -> \n";
				if (commandType is CommandType.C_PUSH)
				{
					pointerAssembly += $"@{segment}\n";
					pointerAssembly += "D=M\n";
					pointerAssembly += Push();
				}
				else
				{
					pointerAssembly += Pop();
					pointerAssembly += $"@{segment}\n";
					pointerAssembly += "M=D\n";
				}
				pointerAssembly += "// <- POINTER PUSH/POP END -> \n";

				return pointerAssembly;
			case "temp":
				if (index < 0 || index > 7)
				{
					throw new ArgumentException($"Code writer error. Index is outside of the range: {index}.");
				}
				string tempAssembly = "// <- BEGIN TEMP PUSH/POP -> \n";
				tempAssembly += "@5\n";
				tempAssembly += "D=A\n";
				tempAssembly += $"@{index}\n";
				tempAssembly += "D=D+A\n";
				tempAssembly += "@R13\n";
				tempAssembly += "M=D\n";
				if (commandType is CommandType.C_PUSH)
				{
					tempAssembly += "@R13\n";
					tempAssembly += "A=M\n";
					tempAssembly += "D=M\n";
					tempAssembly += Push();
				}
				else
				{
					tempAssembly += Pop();
					tempAssembly += "@R13\n";
					tempAssembly += "A=M\n";
					tempAssembly += "M=D\n";
				}
				tempAssembly += "// <- END TEMP PUSH/POP -> \n";
				return tempAssembly;
			case "constant":
				string constantAssembly = "// <- BEGIN CONSTANT PUSH/POP -> \n";
				constantAssembly += $"@{index}\n";
				constantAssembly += "D=A\n";
				constantAssembly += Push();
				constantAssembly += "// <- END CONSTANT PUSH/POP -> \n";
				return constantAssembly;
			case "static":
				string staticAssembly = "// <- BEGIN STATIC PUSH/POP -> \n";
				string staticLabel = $"{fileName}.{index}";
				if (commandType is CommandType.C_PUSH)
				{
					staticAssembly += $"@{staticLabel}\n";
					staticAssembly += "D=M\n";
					staticAssembly += Push();
				}
				else
				{
					staticAssembly += Pop();
					staticAssembly += $"@{staticLabel}\n";
					staticAssembly += "M=D\n";
				}
				staticAssembly += "// <- END STATIC PUSH/POP -> \n";
				return staticAssembly;
			default:
				throw new InvalidOperationException($"Code writer error. Inappropriate segment type: {segment}.");
		}
	}
	private string SegmentPopPush(string segment, int index, CommandType commandType)
	{
		string command = commandType.ToString().Substring(2).ToLower();

		string assembly = $"// <- BEGIN {segment.ToUpper()} PUSH/POP -> \n";
		assembly += $"@{index}\n";
		assembly += "D=A\n";
		assembly += $"@{segment}\n";
		assembly += "A=M\n";
		assembly += "D=D+M\n";
		assembly += "@R13\n";
		assembly += "M=D\n";
		if (command == "push")
		{
			assembly += "@R13\n";
			assembly += "A=M\n";
			assembly += "D=M\n";
			assembly += Push();
		}
		else if (command == "pop")
		{
			assembly += Pop();
			assembly += "@R13\n";
			assembly += "A=M\n";
			assembly += "M=D\n";
		}
		else
		{
			throw new InvalidOperationException($"Code writer error. Inappropriate command type: {command}.");
		}
		assembly += $"// <- END {segment.ToUpper()} PUSH/POP -> \n";

		return assembly;
	}
	private string Add()
	{
		string assembly = "// <- BEGIN ADD -> \n";
		assembly += Pop();
		assembly += Pop("D=D+M\n");
		assembly += Push();
		assembly += "// <- END ADD -> \n";

		return assembly;
	}
	private string Subtract()
	{
		string assembly = "// <- BEGIN SUBTRACT -> \n";
		assembly += Pop();
		assembly += Pop("D=M-D\n");
		assembly += Push();
		assembly += "// <- END SUBTRACT -> \n";

		return assembly;
	}
	private string Negate()
	{
		string assembly = "// <- BEGIN NEGATE -> \n";
		assembly += Pop();
		assembly += "D=-D\n";
		assembly += Push();
		assembly += "// <- END NEGATE -> \n";

		return assembly;
	}
	private string Equal()
	{
		string assembly = "// <- BEGIN EQUAL -> \n";
		(string, string) notEqual = ($"@NOT_EQUAL_{_labelID}\n", $"(NOT_EQUAL_{_labelID})\n");
		(string, string) endNotEqual = ($"@END_NOT_EQUAL_{_labelID}\n", $"(END_NOT_EQUAL_{_labelID})\n");
		_labelID++;

		assembly += Pop();
		assembly += Pop("D=D-M\n");
		assembly += notEqual.Item1;
		assembly += "D;JNE\n";
		assembly += "D=-1\n";
		assembly += Push();
		assembly += endNotEqual.Item1;
		assembly += "0;JMP\n";
		assembly += notEqual.Item2;
		assembly += "D=0\n";
		assembly += Push();
		assembly += endNotEqual.Item2;

		assembly += "// <- END EQUAL -> \n";

		return assembly;
	}
	private string GreaterThan()
	{
		string assembly = "// <- BEGIN GREATER THAN -> \n";
		(string, string) greater = ($"@GREATER_{_labelID}\n", $"(GREATER_{_labelID})\n");
		(string, string) endGreater = ($"@END_GREATER_{_labelID}\n", $"(END_GREATER_{_labelID})\n");
		_labelID++;

		assembly += Pop();
		assembly += Pop("D=D-M\n");
		assembly += greater.Item1;
		assembly += "D;JLT\n";
		assembly += "D=0\n";
		assembly += Push();
		assembly += endGreater.Item1;
		assembly += "0;JMP\n";
		assembly += greater.Item2;
		assembly += "D=-1\n";
		assembly += Push();
		assembly += endGreater.Item2;

		assembly += "// <- END GREATER THAN -> \n";

		return assembly;
	}
	private string LessThan()
	{
		string assembly = "// <- BEGIN LESS THAN -> \n";
		(string, string) less = ($"@LESS_{_labelID}\n", $"(LESS_{_labelID})\n");
		(string, string) endLess = ($"@END_LESS_{_labelID}\n", $"(END_LESS_{_labelID})\n");
		_labelID++;

		assembly += Pop();
		assembly += Pop("D=D-M\n");
		assembly += less.Item1;
		assembly += "D;JLE\n";
		assembly += "D=-1\n";
		assembly += Push();
		assembly += endLess.Item1;
		assembly += "0;JMP\n";
		assembly += less.Item2;
		assembly += "D=0\n";
		assembly += Push();
		assembly += endLess.Item2;

		assembly += "// <- END LESS THAN -> \n";

		return assembly;
	}
	private string And()
	{
		string assembly = "// <- BEGIN AND -> \n";

		assembly += Pop();
		assembly += Pop("D=D&M\n");
		assembly += Push();

		assembly += "// <- END AND -> \n";

		return assembly;
	}
	private string Or()
	{
		string assembly = "// <- BEGIN OR -> \n";

		assembly += Pop();
		assembly += Pop("D=D|M\n");
		assembly += Push();

		assembly += "// <- END OR -> \n";

		return assembly;
	}
	private string Not()
	{
		string assembly = "// <- BEGIN NOT -> \n";

		assembly += Pop("D=!M\n");
		assembly += Push();

		assembly += "// <- END NOT -> \n";

		return assembly;
	}
	private string Push()
	{
		string assembly = "// <- BEGIN PUSH -> \n";

		assembly += "@SP\n";
		assembly += "A=M\n";
		assembly += "M=D\n";
		assembly += "@SP\n";
		assembly += "M=M+1\n";

		assembly += "// <- END PUSH -> \n";

		return assembly;
	}
	private string Pop(string dataManipulation = "D=M\n")
	{
		string assembly = $"// <- BEGIN POP -> \n";

		assembly += "@SP\n";
		assembly += "M=M-1\n";
		assembly += "A=M\n";
		assembly += dataManipulation;

		assembly += "// <- END POP -> \n";

		return assembly;
	}
	public string EndProgram()
	{
		string assembly = "// <- BEGIN END PROGRAM -> \n";

		assembly += "(END)\n";
		assembly += "@END\n";
		assembly += "0;JMP\n";

		assembly += "// <- END END PROGRAM -> \n";

		return assembly;
	}
}