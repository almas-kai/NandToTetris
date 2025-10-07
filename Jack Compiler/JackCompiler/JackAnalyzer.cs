namespace JackCompiler;

internal class JackAnalyzer
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("The program accepts the \'.jack\' file or the path to the folder that contains \'.jack\' files.");
        }
        else
        {
            string potentialPath = args[0].Trim();
            JackFileReader jackFileReader = new JackFileReader(potentialPath);

            if (jackFileReader.IsCorrectPath is false)
            {
                Console.WriteLine("Make sure that the path is correct. The program accepts the \'.jack\' file or the path to the folder that contains \'.jack\' files.");
                return;
            }
        }
    }
}
