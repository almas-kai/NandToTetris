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
	private int _pointer = 0;
	private string[] _instructions;
	public InstructionType CurrentInstructionType { get; private set; }
	private Dictionary<string, int> _symbolTable = new Dictionary<string, int>();
	private Dictionary<string, int> _predefinedSymbols = new Dictionary<string, int>()
	{
		{ "R0", 0 }, { "R1", 1 }, { "R2", 2 }, { "R3", 3 }, { "R4", 4 }, { "R5", 5 }, { "R6", 6 }, { "R7", 7 }, { "R8", 8 }, { "R9", 9 }, { "R10", 10 }, { "R11", 11 }, { "R12", 12 }, { "R13", 13 }, { "R14", 14 }, { "R15", 15 }, { "SP", 0 }, { "LCL", 1 }, { "ARG", 2 }, { "THIS", 3 }, { "THAT", 4 }, { "SCREEN", 16384 }, { "KBD", 24576 }
	};
	private int _variablePointer = 16;
	private bool _isSymbolPopulating = true;
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
		PopulateSymbolTable();
	}
	public void Advance()
	{
		if (HasMoreLines is false)
		{
			throw new InvalidOperationException($"Invalid operation. Can't advance. There is no more instructions to read for. Current instruction number is {_pointer}. Current instruction is {_instructions[_pointer]}.");
		}
		string instructionType = _instructions[_counter];
		_pointer = _counter;
		if (instructionType.StartsWith("@"))
		{
			CurrentInstructionType = InstructionType.A_INSTRUCTION;
			instructionType = instructionType.Substring(1);
			if (_isSymbolPopulating is false && _symbolTable.ContainsKey(instructionType) is false && Validator.IsCorrectSymbol(instructionType))
			{
				_symbolTable.Add(instructionType, _variablePointer);
				_variablePointer++;
			}
		}
		else if (instructionType.StartsWith("(") && instructionType.EndsWith(")"))
		{
			CurrentInstructionType = InstructionType.L_INSTRUCTION;
		}
		else
		{
			CurrentInstructionType = InstructionType.C_INSTRUCTION;
		}
		_counter++;
	}
	public string Symbol()
	{
		if (CurrentInstructionType == InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the symbol or decimal value from this type of instruction. Current instruction number is {_pointer}. Current instruction is {_instructions[_pointer]}.");
		}
		string currentInstruction = _instructions[_pointer];
		if (currentInstruction.StartsWith("(") && currentInstruction.EndsWith(")"))
		{
			string symbol = currentInstruction.Substring(1, currentInstruction.Length - 2);

			if (_symbolTable.TryGetValue(symbol, out int variable))
			{
				return variable.ToString();
			}
			else if (Validator.IsCorrectSymbol(symbol))
			{
				return symbol;
			}
			else
			{
				throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. Couldn't extract the symbol, because the format was incorrect. Current instruction number is {_pointer}. Current instruction is {_instructions[_pointer]}.");
			}
		}
		else if (currentInstruction.StartsWith("@"))
		{
			string address = currentInstruction.Substring(1);
			if (_predefinedSymbols.TryGetValue(address, out int predefinedValue))
			{
				return predefinedValue.ToString();
			}
			else if (_symbolTable.TryGetValue(address, out int variable))
			{
				return variable.ToString();
			}
			else if (Validator.IsCorrectConstant(address) || Validator.IsCorrectSymbol(address))
			{
				return address;
			}
			else
			{
				throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. Couldn't extract the underlying symbol or decimal value, because the format was incorrect. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
			}
		}
		else
		{
			throw new InvalidOperationException($"Something went wrong. The current instruction type is {CurrentInstructionType}. The current instruction number is {_pointer}. The current instruction is: {_instructions[_pointer]}.");
		}
	}
	public string Destination()
	{
		if (CurrentInstructionType != InstructionType.C_INSTRUCTION)
		{
			throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the destination part from this type of the instruction. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
		}
		string destination = _instructions[_pointer];
		if (destination.Contains("="))
		{
			int index = destination.IndexOf("=");
			destination = destination.Substring(0, index);
			if (Regex.IsMatch(destination, @"^(A|D|M|AD|AM|DM|ADM)$") is false)
			{
				throw new InvalidOperationException($"Invalid operation. The destination part is not correct. The destination part is {destination}. The instruction number is {_pointer}. The instruction is {_instructions[_pointer]}.");
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
			throw new InvalidOperationException($"Invalid operation. The instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the computational part. The current instruction type is {_pointer}. The current instruction is {_instructions[_pointer]}.");
		}
		string computational = _instructions[_pointer];
		if (computational.Contains("="))
		{
			Match match = Regex.Match(computational, @"=(.*?)(?:;|$)");
			if (match.Success is false)
			{
				throw new InvalidOperationException($"Something went wrong. The syntax was incorrect. Tried to capture the computational part after the '=' sign but couldn't do it. The current instruction type is {CurrentInstructionType}. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
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
			throw new InvalidOperationException($"Invalid operation. Instruction type error. The current instruction type is {CurrentInstructionType}. But it has to be {InstructionType.C_INSTRUCTION}. Can't extract the 'jump' part. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
		}
		string jump = _instructions[_pointer];
		if (jump.Contains(";"))
		{
			int startIndex = jump.IndexOf(";");
			jump = jump.Substring(startIndex);
		}
		else
		{
			jump = "";
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
	private void PopulateSymbolTable()
	{
		if (_counter != 0)
		{
			throw new InvalidOperationException($"Something went wrong. The symbol table must be populated before the read of any other instructions. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
		}
		if (HasMoreLines)
		{
			Advance();
			string instruction = _instructions[_pointer];
			if (instruction.StartsWith("(") && instruction.EndsWith(")"))
			{
				string label = instruction.Substring(1, instruction.Length - 2);
				if (Validator.IsCorrectConstant(label) is false)
				{
					if (Validator.IsCorrectSymbol(label) is true)
					{
						_symbolTable.Add(label, _pointer);
					}
					else
					{
						throw new InvalidOperationException($"Something went wrong. The label symbol was in incorrect format. The label symbol was {label}. The instruction number is {_pointer}. The whole instruction is {_instructions[_pointer]}.");
					}
				}
			}
		}
		_counter = 0;
		_pointer = 0;
		_isSymbolPopulating = false;
	}
}