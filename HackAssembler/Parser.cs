using System.Text.RegularExpressions;

namespace HackAssembler;

public enum InstructionType
{
	A_INSTRUCTION,
	C_INSTRUCTION,
	L_INSTRUCTION
}
public class Parser
{
	private int _counter = 0;
	private string[] _instructions;
	public InstructionType InstructionType { get; private set; }
	public Parser(FileInfo file)
	{
		using (StreamReader _streamReader = file.OpenText())
		{
			_instructions = _streamReader.ReadToEnd()
				.Split("\n")
				.Where(instruction => Regex.IsMatch(instruction, @"^\s*(//.*)?$") is false)
				.Select(instruction => Regex.Replace(instruction, @"\s", ""))
				.ToArray();
		}
	}
	public void Advance()
	{
		if (HasMoreLines is false)
		{
			throw new InvalidOperationException($"Invalid operation. Can't advance. There is no more instructions to read for. Current instruction number is {_counter}. Current instruction is {_instructions[_counter]}.");
		}
		_counter++;
		char instructionType = _instructions[_counter][0];
		switch (instructionType)
		{
			case '@':
				InstructionType = InstructionType.A_INSTRUCTION;
				break;
			case '(':
				InstructionType = InstructionType.L_INSTRUCTION;
				break;
			default:
				InstructionType = InstructionType.C_INSTRUCTION;
				break;
		}
	}
	public string Symbol()
	{
		if (InstructionType == InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {InstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the symbol or decimal value from this type of instruction. Current instruction number is {_counter}. Current instruction is {_instructions[_counter]}.");
		}
		string currentInstruction = _instructions[_counter];
		if (currentInstruction.StartsWith("(") && currentInstruction.EndsWith(")"))
		{
			string symbol = currentInstruction.Substring(1, currentInstruction.Length - 2);
			if (IsCorrectSymbol(symbol))
			{
				return symbol;
			}
			else
			{
				throw new InvalidOperationException($"Invalid operation. The instruction type is {InstructionType}. Couldn't extract the symbol, because the format was incorrect. Current instruction number is {_counter}. Current instruction is {_instructions[_counter]}.");
			}
		}
		else if (currentInstruction.StartsWith("@"))
		{
			string address = currentInstruction.Substring(1);
			if (IsCorrectConstant(address) || IsCorrectSymbol(address))
			{
				return address;
			}
			else
			{
				throw new InvalidOperationException($"Invalid operation. The instruction type is {InstructionType}. Couldn't extract the underlying symbol or decimal value, because the format was incorrect. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
			}
		}
		else
		{
			throw new InvalidOperationException($"Something went wrong. The current instruction type is {InstructionType}. The current instruction number is {_counter}. The current instruction is: {_instructions[_counter]}.");
		}
	}
	public string Destination()
	{
		if (InstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {InstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the destination part from this type of the instruction. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
		string destination = _instructions[_counter].Substring(10, 3);
		switch (destination)
		{
			case "000":
				return String.Empty;
			case "001":
				return "M";
			case "010":
				return "D";
			case "011":
				return "DM";
			case "100":
				return "A";
			case "101":
				return "AM";
			case "110":
				return "AD";
			case "111":
				return "ADM";
			default:
				throw new InvalidOperationException($"Something went wrong. The destiantion command is {destination}. The instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
	}
	public string Computation()
	{
		if (InstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {InstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the computational part. The current instruction type is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
		string computational = _instructions[_counter].Substring(4, 6);
		char a = _instructions[_counter][3];
		if (a == '0')
		{
			switch (computational)
			{
				case "101010":
					return "0";
				case "111111":
					return "1";
				case "111010":
					return "-1";
				case "001100":
					return "D";
				case "110000":
					return "A";
				case "001101":
					return "!D";
				case "110001":
					return "!A";
				case "001111":
					return "-D";
				case "110011":
					return "-A";
				case "011111":
					return "D+1";
				case "110111":
					return "A+1";
				case "001110":
					return "D-1";
				case "110010":
					return "A-1";
				case "000010":
					return "D+A";
				case "010011":
					return "D-A";
				case "000111":
					return "A-D";
				case "000000":
					return "D&A";
				case "010101":
					return "D|A";
				default:
					throw new InvalidOperationException($"Something went wrong. 'a' is {a}. The command is wrong. Computational command is {computational}. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
			}
		}
		else if (a == '1')
		{
			switch (computational)
			{
				case "110000":
					return "M";
				case "110001":
					return "!M";
				case "110011":
					return "-M";
				case "110111":
					return "M+1";
				case "110010":
					return "M-1";
				case "000010":
					return "D+M";
				case "010011":
					return "D-M";
				case "000111":
					return "M-D";
				case "000000":
					return "D&M";
				case "010101":
					return "D|M";
				default:
					throw new InvalidOperationException($"Something went wrong. The 'a' is {a}. The command is wrong. The computational command is {computational}. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
			}
		}
		else
		{
			throw new InvalidOperationException($"Something went wrong. The 'a' instruction is {a}. The computational instruction is {computational}. The instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
	}
	public string Jump()
	{
		if (InstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. Instruction type error. The current instruction type is {InstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the 'jump' part. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
		string jump = _instructions[_counter].Substring(13);
		switch (jump)
		{
			case "000":
				return String.Empty;
			case "001":
				return "JGT";
			case "010":
				return "JEQ";
			case "011":
				return "JGE";
			case "100":
				return "JLT";
			case "101":
				return "JNE";
			case "110":
				return "JLE";
			case "111":
				return "JMP";
			default:
				throw new InvalidOperationException($"Something went wrong. Non-supported 'jump' command. It is {jump}. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
	}
	public bool HasMoreLines
	{
		get
		{
			return _counter < _instructions.Length;
		}
	}
	private bool IsCorrectSymbol(string symbol)
	{
		return Regex.IsMatch(symbol, @"^[a-zA-Z._$:][a-zA-Z0-9._$:]*$");
	}
	private bool IsCorrectConstant(string constant)
	{
		if (ushort.TryParse(constant, out ushort number))
		{
			return 0 <= number && 32767 >= number;
		}
		return false;
	}
}