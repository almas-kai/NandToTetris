namespace VMTranslator;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Please enter the path to the assembly file (FileName.vm).");
        }
        else
        {
            string path = args[0].Trim();
            if (path.EndsWith(".vm") is false)
            {
                Console.WriteLine("The file has to have the \".vm\" extension.");
            }
            else if (File.Exists(path) is false)
            {
                Console.WriteLine("The file doesn't exist. Or you don't have the permission to read the file.");
            }
            else
            {
                FileInfo inputFile = new FileInfo(path);
                string outputFile = inputFile.FullName.Replace(".vm", ".asm");
                using (StreamWriter streamWriter = new StreamWriter(outputFile))
                {
                }
            }
        }
    }
}
