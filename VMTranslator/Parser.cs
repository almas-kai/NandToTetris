using System.Text.RegularExpressions;

namespace VMTranslator;

public enum CommandType
{
	C_ARITHMETIC,
	C_PUSH,
	C_POP,
	C_LABEL,
	C_GOTO,
	C_IF,
	C_FUNCTION,
	C_RETURN,
	C_CALL
}
public class Parser
{
	private string[] _instructions;
	private int _pointer = -1;
	public CommandType CurrentCommandType;
	public Parser(FileInfo file)
	{
		using (StreamReader streamReader = file.OpenText())
		{
			_instructions = streamReader.ReadToEnd()
				.Split("\n")
				.Where(instruction => Regex.IsMatch(instruction, @"^\s*(//.*)?$") is false)
				.Select(instruction =>
				{
					instruction = Regex.Replace(instruction, @"\s", "");
					instruction = Regex.Replace(instruction, @"//.*", "");
					return instruction;
				})
				.ToArray();
		}
	}
	public bool HasMoreLines()
	{
		return _pointer < _instructions.Length;
	}
	public void Advance()
	{
		_pointer++;
		if (HasMoreLines() is true)
		{
			string command = _instructions[_pointer];
			if (command.StartsWith("push"))
			{
				CurrentCommandType = CommandType.C_PUSH;
			}
			else if (command.StartsWith("pop"))
			{
				CurrentCommandType = CommandType.C_POP;
			}
			else if (Regex.IsMatch(command, @"^(add|sub|neg|eq|gt|lt|and|or|not)$"))
			{
				CurrentCommandType = CommandType.C_ARITHMETIC;
			}
			else if (command.StartsWith("label"))
			{
				CurrentCommandType = CommandType.C_LABEL;
			}
			else if (command.StartsWith("goto"))
			{
				CurrentCommandType = CommandType.C_GOTO;
			}
			else if (command.StartsWith("if-goto"))
			{
				CurrentCommandType = CommandType.C_IF;
			}
		}
	}
	public string GetArg1()
	{
		if (CurrentCommandType is CommandType.C_RETURN)
		{
			throw new InvalidOperationException($"Parser error. Cannot call for the command type {CurrentCommandType}. The instruction number is {_pointer}. The instruction is {_instructions[_pointer]}.");
		}
		switch (CurrentCommandType)
		{
			case CommandType.C_ARITHMETIC:
				return _instructions[_pointer];
			case CommandType.C_POP or CommandType.C_PUSH:
				string p_command = _instructions[_pointer];
				if (TryExtractSegment(p_command, out string segment))
				{
					return segment;
				}
				throw new InvalidOperationException($"Parser error. Cannot extract the segment part of the push/pop instruction. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
			case CommandType.C_LABEL:
				string l_command = _instructions[_pointer];
				if (TryExtractLabel(l_command, out string c_label))
				{
					return c_label;
				}
				throw new InvalidOperationException($"Parser error. Cannot extract the label part of the label instruction. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
			case CommandType.C_GOTO:
				string g_command = _instructions[_pointer];
				if (TryExtractLabel(g_command, out string g_label))
				{
					return g_label;
				}
				throw new InvalidOperationException($"Parser error. Cannot extract the label part of the goto instruction. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
			case CommandType.C_IF:
				string if_command = _instructions[_pointer];
				if (TryExtractLabel(if_command, out string if_label))
				{
					return if_label;
				}
				throw new InvalidOperationException($"Parser error. Cannot extract the label part of the if-goto instruction. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
			default:
				throw new InvalidOperationException($"Parser error. Cannot extract the arg1. Because of the unknown command type: {CurrentCommandType}. The instruction number is {_pointer}. The instruction is {_instructions[_pointer]}.");
		}
	}
	public string GetArg2()
	{
		if (CurrentCommandType is not CommandType.C_PUSH && CurrentCommandType is not CommandType.C_POP && CurrentCommandType is not CommandType.C_FUNCTION && CurrentCommandType is not CommandType.C_CALL)
		{
			throw new InvalidOperationException($"Parser error. Cannot extract the second argument because of the wrong command type: {CurrentCommandType}. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
		}
		switch (CurrentCommandType)
		{
			case CommandType.C_PUSH or CommandType.C_POP:
				string command = _instructions[_pointer];
				if (TryExtractSegmentIndex(command, out int index))
				{
					return index.ToString();
				}
				throw new InvalidOperationException($"Parser error. Cannot extract the index. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
			default:
				throw new InvalidOperationException($"Parser error. Cannot extract the arg2. Because of the unknown command type: {CurrentCommandType}. The instruction number is {_pointer}. The instruction is {_instructions[_pointer]}.");
		}
	}
	private bool TryExtractSegment(string command, out string segment)
	{
		bool result = false;
		segment = String.Empty;
		Match match = Regex.Match(command, @"(argument|local|static|constant|this|that|pointer|temp)");
		if (match.Success)
		{
			segment = match.Value;
			result = true;
		}
		return result;
	}
	private bool TryExtractSegmentIndex(string command, out int index)
	{
		index = -1;
		bool result = false;
		Match match = Regex.Match(command, @"(\d+)$");
		if (match.Success)
		{
			string number = match.Value;
			if (int.TryParse(number, out int value))
			{
				result = value > -1;
				index = value;
			}
		}
		return result;
	}
	private bool TryExtractLabel(string fullCommand, out string label)
	{
		int offset = 0;
		bool result = false;
		label = "";
		switch (CurrentCommandType)
		{
			case CommandType.C_LABEL:
				offset = 5;
				break;
			case CommandType.C_GOTO:
				offset = 4;
				break;
			case CommandType.C_IF:
				offset = 7;
				break;
			default:
				throw new InvalidOperationException($"Parser error. Extracting the label on the {CurrentCommandType} is forbidden. The instruction number is: {_pointer}. The instruction is {_instructions[_pointer]}.");
		}
		label = fullCommand.Substring(offset);
		if (Regex.IsMatch(label, @"^[a-zA-Z._:][a-zA-Z0-9._:]*$"))
		{
			result = true;
		}
		return result;
	}
}