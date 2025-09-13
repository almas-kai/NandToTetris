namespace JackCompiler;

class JackAnalyzer
{
    private const string JACK_EXTENSION = ".jack";
    private const string XML_EXTENSION = ".xml";
    private const string TOKENIZED_OUTPUT = "T";
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("The program accepts the \'.jack\' file or the path to the folder that contains \'.jack\' files.");
        }
        else
        {
            string potentialPath = args[0].Trim();
            List<string> filesToCompile = new List<string>();
            if (Directory.Exists(potentialPath))
            {
                filesToCompile.AddRange(Directory.GetFiles(potentialPath).Where(f => f.EndsWith(JACK_EXTENSION)));
            }
            else if (potentialPath.EndsWith(JACK_EXTENSION) && File.Exists(potentialPath))
            {
                filesToCompile.Add(potentialPath);
            }
            if (filesToCompile.Count == 0)
            {
                Console.WriteLine("Make sure that the path is correct. The program accepts the \'.jack\' file or the path to the folder that contains \'.jack\' files.");
                return;
            }
            foreach (string fileName in filesToCompile)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                JackTokenizer jackTokenizer = new JackTokenizer(fileInfo);

                string outputPathForTestingXMLFile = fileName.Replace(JACK_EXTENSION, TOKENIZED_OUTPUT + XML_EXTENSION);

                // Testing generation must be called before any Advance calls.
                jackTokenizer.GenerateTestingXMLFile(outputPathForTestingXMLFile);

                string outputFileName = fileName.Replace(JACK_EXTENSION, XML_EXTENSION);
            }
        }
    }
}
