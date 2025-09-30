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
            List<string> filesToCompile = new List<string>();
            if (Directory.Exists(potentialPath))
            {
                filesToCompile.AddRange(Directory.GetFiles(potentialPath).Where(f => f.EndsWith(FileExtensions.JACK_EXTENSION)));
            }
            else if (potentialPath.EndsWith(FileExtensions.JACK_EXTENSION) && File.Exists(potentialPath))
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

                string outputPathForTestingXMLFile = fileName.Replace(FileExtensions.JACK_EXTENSION, FileExtensions.TOKENIZED_OUTPUT + FileExtensions.XML_EXTENSION);

                // Testing generation must be called before any Advance calls.
                jackTokenizer.GenerateTestingXMLFile(outputPathForTestingXMLFile);

                string outputFileName = fileName.Replace(FileExtensions.JACK_EXTENSION, FileExtensions.XML_EXTENSION);
            }
        }
    }
}
