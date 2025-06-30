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
				.Select(instruction => Regex.Replace(instruction, @"\s", ""))
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
				string command = _instructions[_pointer];
				if (TryExtractSegment(command, out string segment))
				{
					return segment;
				}
				throw new InvalidOperationException($"Parser error. Cannot extract the segment part of the instruction. The current instruction number is {_pointer}. The current instruction is {_instructions[_pointer]}.");
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
}