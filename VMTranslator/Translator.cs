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
                Parser parser = new Parser(inputFile);
                CodeWriter codeWriter = new CodeWriter();
                using (StreamWriter streamWriter = new StreamWriter(outputFile))
                {
                    parser.Advance();
                    while (parser.HasMoreLines())
                    {
                        string asmCode = "";
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
                        streamWriter.WriteLine(asmCode);
                        parser.Advance();
                    }
                    streamWriter.WriteLine(codeWriter.EndProgram());
                }
            }
        }
    }
}
