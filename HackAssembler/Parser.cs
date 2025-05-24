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
	private readonly FileInfo _file;
	private int _counter = 0;
	private string[] _instructions;
	public InstructionType InstructionType { get; private set; }
	public Parser(FileInfo file)
	{
		_file = file;
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
			throw new InvalidOperationException("Can't advance. There is no more instructions to read for.");
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
			throw new InvalidOperationException($"The instruction type is {InstructionType}. Can't extract the symbol or decimal value.");
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
				throw new InvalidOperationException($"The instruction type is {InstructionType}. Couldn't extract the symbol, because the format was incorrect.");
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
				throw new InvalidOperationException($"The instruction type is {InstructionType}. Couldn't extract the underlying symbol or decimal value, because the format was incorrect.");
			}
		}
		else
		{
			throw new InvalidOperationException($"Something went wrong. The current instruction type is {InstructionType}. The current instruction is: {currentInstruction}.");
		}
	}
	public string Destination()
	{
		if (InstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"The instruction type is {InstructionType}. Can't extract the destination part.");
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