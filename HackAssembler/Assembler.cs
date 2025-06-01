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
			Parser parser = new Parser(new FileInfo(path));
		}
	}
}