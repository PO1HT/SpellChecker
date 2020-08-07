using System;
using System.IO;
using System.Collections.Generic;
using Redaction;
using WorkArgs;

using System.Diagnostics;

namespace SpellCheckerIO
{
    class SpellCheсker
    {
        public string inputFullFileName;
        public string otputFullFileName;
        public string inputDictWords = ""; //dictionary by a sequence of;
        public string inputPossibleMisspletWords = ""; //Possible Misspelt Words;
        public string countWordsIntoDictWords = ""; 
        public string countWordsIntoPMWords = ""; 
        public string countWordsIntoEachLines; //количество слов в каждой строке
        public bool fileIsLoaded = false;

        public bool fileOpenAfterWrite = false;
        
        public SpellCheсker(string[] args)
        {
            WorkWithArgs workWithArgs = new WorkWithArgs();
            this.inputFullFileName = workWithArgs.InputFilename(args);
            this.otputFullFileName = workWithArgs.OutputFilename(args);
            ReadedSourceFile();
        }

        public void ReadedSourceFile()
        {
            if (File.Exists(inputFullFileName)) 
            {
                ReadingSourceFile();
                fileIsLoaded = true;
            }
            else    
                fileIsLoaded = false;
        }

        public string AddInputWord(string destanation, string newPMWord)
        {

            //inputPossibleMisspletWords
            if (destanation.Length != 0)
                if (destanation[destanation.Length-1] == ' ' )
                    return destanation +  newPMWord;
                else    
                    return destanation + ' ' + newPMWord;
            else
                return destanation + newPMWord;
        }
        public bool IsNotSepparator(string match)
        {
            string sepparator = "===";
            if (String.Compare(match.Trim(), sepparator) != 0)
                return true;
            else
                return false;
        }//may be add IsNotNull (The end of file) написано что бы вернуться позже к этому вопросу
        
        public string AddSourceLineToDestanation(string destanation, string line)
        {
            int countWordsInCurrentLine = 0;
            foreach(string currentWord in line.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries))
            {
                countWordsInCurrentLine++;
                destanation = AddInputWord(destanation, currentWord);
            }
            countWordsIntoEachLines = countWordsIntoEachLines + countWordsInCurrentLine.ToString();
            return destanation;
        }
        public String ReadPartFile(StreamReader inputFile)
        {
            countWordsIntoEachLines = "";
            String line;
            String destanation ="";
            while (IsNotSepparator(line = inputFile.ReadLine())) 
            {
                destanation = AddSourceLineToDestanation(destanation, line);
            }
            return destanation;
        }
        public void ReadInputFile(StreamReader inputFile)
        {
            inputDictWords = ReadPartFile(inputFile);
            countWordsIntoDictWords =  countWordsIntoEachLines;
            inputPossibleMisspletWords = ReadPartFile(inputFile);
            countWordsIntoPMWords = countWordsIntoEachLines;
        }
        public void ReadingSourceFile()
        {
            try
                {
                using (StreamReader inputFile = new StreamReader(@inputFullFileName))
                    {
                        ReadInputFile(inputFile);
                    }
                }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }  
        }
    
        public void WriteResults(string[] result_words)
        {
            const int offsetOfNumbericChar = 48;
            StreamWriter Outf = new StreamWriter(otputFullFileName);
            int curr_print_elem = 0;
            int the_end = 0;
            int[] arr_r = new int[countWordsIntoPMWords.Length];
            int curr_elem_arr_r = 0;
            //Console.WriteLine(countWordsIntoPMWords);
            for (int i = 0;i < countWordsIntoPMWords.Length;i++)
            {
                //arr_r[i] = Convert.ToInt32(countWordsIntoPMWords[i]) - offsetOfNumbericChar;
                the_end = the_end + Convert.ToInt16(countWordsIntoPMWords[i]) - offsetOfNumbericChar;
                arr_r[i] = the_end;
            }
            //Console.WriteLine("{0} {1} {2}",arr_r[0],arr_r[1],arr_r[2]);
            curr_elem_arr_r = 0;
            while (curr_print_elem < the_end)
            {
                if (arr_r[curr_elem_arr_r] == curr_print_elem)
                {
                    curr_elem_arr_r++;
            //        Console.WriteLine();
                    Outf.WriteLine();
                }
            //    Console.Write("{0} ",result_words[curr_print_elem]);
                Outf.Write("{0} ",result_words[curr_print_elem]);
                curr_print_elem++;
            }
            Outf.Close();
            if (fileOpenAfterWrite)
            {
                OpenNotepadResults();
            }    
        }
        
        public void OpenNotepadResults()
        {
            Process.Start("notepad.exe", otputFullFileName);
        }
    }

}