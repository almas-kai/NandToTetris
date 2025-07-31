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
            string potentialPath = args[0].Trim();
            string outputFileName = "";
            List<string> filesToTranslate = new List<string>();
            if (Directory.Exists(potentialPath))
            {
                outputFileName = potentialPath + (potentialPath.EndsWith("/") ? "" : "/") + "source.asm";
                filesToTranslate.AddRange(Directory.GetFiles(potentialPath).Where(f => f.EndsWith(".vm")));
            }
            else if (potentialPath.EndsWith(".vm") && File.Exists(potentialPath))
            {
                outputFileName = potentialPath.Replace(".vm", ".asm");
                filesToTranslate.Add(potentialPath);
            }
            if (filesToTranslate.Count == 0)
            {
                Console.WriteLine("Enter a directory or a \".vm\" file. The directory must contain at least one \".vm\" file.");
                return;
            }
            CodeWriter codeWriter = new CodeWriter();
            string asmCode = "";
            using (StreamWriter streamWriter = new StreamWriter(outputFileName))
            {
                // Remove bootstrapping code for tests that set it automatically, because they might set the values to different that the default ones. This code is needed for FibonacciElement tests and for StaticsTests. For other types of tests it will break them I believe, though i am not sure.
                asmCode = codeWriter.writeBootstrappingCode();
                streamWriter.WriteLine(asmCode);
                foreach (string path in filesToTranslate)
                {
                    FileInfo inputFile = new FileInfo(path);
                    Parser parser = new Parser(inputFile);
                    parser.Advance();
                    while (parser.HasMoreLines())
                    {
                        CommandType commandType = parser.CurrentCommandType;
                        if (commandType is not CommandType.C_RETURN)
                        {
                            string arg1 = parser.GetArg1();
                            if (commandType is CommandType.C_PUSH || commandType is CommandType.C_POP || commandType is CommandType.C_FUNCTION || commandType is CommandType.C_CALL)
                            {
                                string arg2 = parser.GetArg2();
                                if (commandType is CommandType.C_PUSH || commandType is CommandType.C_POP)
                                {
                                    if (int.TryParse(arg2, out int index))
                                    {
                                        asmCode = codeWriter.WritePushPop(commandType, arg1, index, inputFile.Name.Replace(".vm", ""));
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Translator error. The index is not and integer: {arg2}.");
                                    }
                                }
                                else
                                {
                                    int varOrArg = int.Parse(arg2);
                                    if (commandType is CommandType.C_FUNCTION)
                                    {
                                        asmCode = codeWriter.writeFunction(arg1, varOrArg);
                                    }
                                    else if (commandType is CommandType.C_CALL)
                                    {
                                        asmCode = codeWriter.writeCall(arg1, varOrArg);
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Translator error. The command type is not a function or call: {commandType}.");
                                    }
                                }
                            }
                            else if (commandType is CommandType.C_ARITHMETIC)
                            {
                                asmCode = codeWriter.WriteArithmetic(arg1);
                            }
                            else if (commandType is CommandType.C_LABEL)
                            {
                                asmCode = codeWriter.writeLabel(arg1);
                            }
                            else if (commandType is CommandType.C_GOTO)
                            {
                                asmCode = codeWriter.writeGoto(arg1);
                            }
                            else if (commandType is CommandType.C_IF)
                            {
                                asmCode = codeWriter.writeIf(arg1);
                            }
                        }
                        else if (commandType is CommandType.C_RETURN)
                        {
                            asmCode = codeWriter.writeReturn();
                        }
                        else
                        {
                            throw new InvalidOperationException($"Translator error. Unrecognized command type: {commandType}.");
                        }
                        streamWriter.WriteLine(asmCode);
                        parser.Advance();
                    }
                    streamWriter.WriteLine(codeWriter.EndProgram());
                }
            }
        }
    }
}
