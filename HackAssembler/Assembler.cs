namespace HackAssembler;

public class Assembler
{
	static void Main(string[] args)
	{
		if (args.Length != 1)
		{
			Console.WriteLine("Please enter the path to the assembly file (file_name.asm).");
		}
		else
		{
			string path = args[0].Trim();
			if (path.EndsWith(".asm") is false)
			{
				Console.WriteLine("The file has to have the \".asm\" extension.");
			}
			else if (File.Exists(path) is false)
			{
				Console.WriteLine("The file doesn't exist. Or you don't have the permission to read the file.");
			}
			else
			{
				FileInfo inputFile = new FileInfo(path);
				Parser parser = new Parser(inputFile);
				CodeModule codeModule = new CodeModule();

				string outputFile = inputFile.FullName.Replace(".asm", "") + ".hack";
				using (StreamWriter streamWriter = new StreamWriter(outputFile))
				{
					while (parser.HasMoreLines)
					{
						parser.Advance();
						InstructionType instructionType = parser.CurrentInstructionType;
						if (instructionType is InstructionType.A_INSTRUCTION || instructionType is InstructionType.L_INSTRUCTION && parser.IsLoopLabel() is false)
						{
							string symbol = parser.Symbol();
							string binarySymbol = codeModule.GetBinaryOfConstant(symbol);
							streamWriter.WriteLine(binarySymbol);
						}
						else if(instructionType is InstructionType.C_INSTRUCTION)
						{
							string output = "111";
							string symbolicComputational = parser.Computation();
							string binaryComputational = codeModule.Computational(symbolicComputational);
							output += binaryComputational;

							string symbolicDestination = parser.Destination();
							string binaryDestination = codeModule.Destination(symbolicDestination);
							output += binaryDestination;

							string symbolicJump = parser.Jump();
							string binaryJump = codeModule.Jump(symbolicJump);
							output += binaryJump;

							streamWriter.WriteLine(output);
						}
					}
				}
			}
		}
	}
}