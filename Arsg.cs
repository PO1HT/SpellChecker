using System;
using System.IO;

namespace WorkArgs
{
    public class WorkWithArgs
    {

        static bool IsArgsHere(string[] args)
        {
            if (args.Length != 0)
                return true;
            else
                return false;
        }

        static bool IsArgsTwo(string[] args)
        {
            if ((args.Length == 2) & (IsArgsHere(args)))
                return true;
            else
                return false;
        }

        static string CurrentDir()
        {
            return Directory.GetCurrentDirectory() + @"\";
        }
        public string InputFilename(string[] args)
        {
            const string defaultInputFileName = "Input.txt";
            if (IsArgsHere(args))
                return CurrentDir() + args[0];  
            else
                return CurrentDir() +  defaultInputFileName;
        }

        public string OutputFilename(string[] args)
        {
            const string defaultOutputFileName = "Output.txt";
            if (IsArgsTwo(args))
                return CurrentDir() + args[1];  
            else    
                return CurrentDir() + defaultOutputFileName;
        }
    }
}