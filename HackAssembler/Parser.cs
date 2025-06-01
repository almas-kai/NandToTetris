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
	public InstructionType CurrentInstructionType { get; private set; }
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
		char instructionType = _instructions[_counter][0];
		switch (instructionType)
		{
			case '@':
				CurrentInstructionType = InstructionType.A_INSTRUCTION;
				break;
			case '(':
				CurrentInstructionType = InstructionType.L_INSTRUCTION;
				break;
			default:
				CurrentInstructionType = InstructionType.C_INSTRUCTION;
				break;
		}
		_counter++;
	}
	public string Symbol()
	{
		if (CurrentInstructionType == InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the symbol or decimal value from this type of instruction. Current instruction number is {_counter}. Current instruction is {_instructions[_counter]}.");
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
				throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. Couldn't extract the symbol, because the format was incorrect. Current instruction number is {_counter}. Current instruction is {_instructions[_counter]}.");
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
				throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. Couldn't extract the underlying symbol or decimal value, because the format was incorrect. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
			}
		}
		else
		{
			throw new InvalidOperationException($"Something went wrong. The current instruction type is {CurrentInstructionType}. The current instruction number is {_counter}. The current instruction is: {_instructions[_counter]}.");
		}
	}
	public string Destination()
	{
		if (CurrentInstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the destination part from this type of the instruction. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
		string destination = _instructions[_counter];
		if (destination.Contains("="))
		{
			int index = destination.IndexOf("=");
			destination = destination.Substring(0, index);
			if (Regex.IsMatch(destination, @"^(A|D|M|AD|AM|DM|ADM)$") is false)
			{
				throw new InvalidOperationException($"Invalid operation. The destination part is not correct. The destination part is {destination}. The instruction number is {_counter}. The instruction is {_instructions[_counter]}.");
			}
		}
		else
		{
			destination = "";
		}
		return destination;
	}
	public string Computation()
	{
		if (CurrentInstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the computational part. The current instruction type is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
		string computational = _instructions[_counter];
		if (computational.Contains("="))
		{
			Match match = Regex.Match(computational, @"=(.*?)(?:;|$)");
			if (match.Success is false)
			{
				throw new InvalidOperationException($"Something went wrong. The syntax was incorrect. Tried to capture the computational part after the '=' sign but couldn't do it. The current instruction type is {CurrentInstructionType}. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
			}
			computational = match.Groups[1].Value;
		}
		else if (computational.Contains(";"))
		{
			int endIndex = computational.IndexOf(";");
			computational = computational.Substring(0, endIndex);
		}
		return computational;
	}
	public string Jump()
	{
		if (CurrentInstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. Instruction type error. The current instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the 'jump' part. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
		string jump = _instructions[_counter];
		if (jump.Contains(";"))
		{
			int startIndex = jump.IndexOf(";");
			jump = jump.Substring(startIndex);
		}
		else
		{
			throw new InvalidOperationException($"Something went wrong. There is no jump instruction to extract. The syntax is incorrect. The current instruction type is {CurrentInstructionType}. The current instruction number is {_counter}. The current instruction is {_instructions[_counter]}.");
		}
		return jump;
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