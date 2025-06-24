using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;

namespace VMTranslator;

public class CodeWriter
{
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
	public string WritePushPop(CommandType commandType, string segment, int index)
	{
		if (commandType is not CommandType.C_PUSH || commandType is not CommandType.C_POP)
		{
			throw new InvalidOperationException($"Code writer error. Inappropriate command type: {commandType}. It has to be push or pop command.");
		}
		switch (segment)
		{
			case "argument" or "local" or "this" or "that":
				return "";
			default:
				throw new InvalidOperationException($"Code writer error. Inappropriate segment type: {segment}.");
		}
	}
	private string SegmentPopPush(string segment, int index, CommandType commandType)
	{
		string command = commandType.ToString().Substring(2).ToLower();
		string comment = $"// Performing {command} on {segment}[base + {index}].\n";
		string assembly = comment;

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
		return assembly;
	}
	private string Add()
	{
		string comment = "// Adding the two topmost values.\n";
		string assembly = comment;

		assembly += Pop();
		assembly += Pop("D=D+M\n");
		assembly += Push();

		return assembly;
	}
	private string Subtract()
	{
		string comment = "// Subtracting the two topmost values.\n";
		string assembly = comment;

		assembly += Pop();
		assembly += Pop("D=M-D\n");
		assembly += Push();

		return assembly;
	}
	private string Negate()
	{
		string comment = "// Negating the topmost value.\n";
		string assembly = comment;

		assembly += Pop();
		assembly += "D=-D\n";
		assembly += Push();

		return assembly;
	}
	private string Equal()
	{
		string comment = "// Are the two topmost values equal to each other?\n";
		string assembly = comment;
		(string, string) notEqual = ("@NOT_EQUAL\n", "(NOT_EQUAL)\n");
		(string, string) endNotEqual = ("@END_NOT_EQUAL\n", "(END_NOT_EQUAL)\n");

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

		return assembly;
	}
	private string GreaterThan()
	{
		string comment = "// Is x greater than y?\n";
		string assembly = comment;
		(string, string) greater = ("@GREATER\n", "(GREATER)\n");
		(string, string) endGreater = ("@END_GREATER\n", "(END_GREATER)\n");

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

		return assembly;
	}
	private string LessThan()
	{
		string comment = "// Is x less than y?\n";
		string assembly = comment;
		(string, string) less = ("@LESS\n", "(LESS)\n");
		(string, string) endLess = ("@END_LESS\n", "(END_LESS)\n");

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

		return assembly;
	}
	private string And()
	{
		string comment = "// x AND y.\n";
		string assembly = comment;

		assembly += Pop();
		assembly += Pop("D=D&M\n");
		assembly += Push();

		return assembly;
	}
	private string Or()
	{
		string comment = "// x OR y.\n";
		string assembly = comment;

		assembly += Pop();
		assembly += Pop("D=D|M\n");
		assembly += Push();

		return assembly;
	}
	private string Not()
	{
		string comment = "// NOT y.\n";
		string assembly = comment;

		assembly += Pop("D=!M\n");
		assembly += Push();

		return assembly;
	}
	private string Push()
	{
		string comment = "// Pushing D to the stack.\n";
		string assembly = comment;

		assembly += "@SP\n";
		assembly += "A=M\n";
		assembly += "M=D\n";
		assembly += "@SP\n";
		assembly += "M=M+1\n";

		return assembly;
	}
	private string Pop(string dataManipulation = "D=M\n")
	{
		string comment = $"// Popping from the stack. With the manipulation {dataManipulation}.\n";
		string assembly = comment;

		assembly += "@SP\n";
		assembly += "M=M-1\n";
		assembly += "A=M\n";
		assembly += dataManipulation;

		return assembly;
	}
	public string EndProgram()
	{
		string comment = "// Ending loop.\n";
		string assembly = comment;

		assembly += "(END)\n";
		assembly += "@END\n";
		assembly += "0;JMP\n";

		return assembly;
	}
}