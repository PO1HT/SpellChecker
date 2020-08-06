using System;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using Redaction;

namespace TwoEdits
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

    class spellCheсker
    {
        public string inputFullFileName;
        public string otputFullFileName;
        public string inputDictWords = ""; //dictionary by a sequence of;
        public string inputPossibleMisspletWords = ""; //Possible Misspelt Words;
        public string countWordsIntoDictWords = ""; 
        public string countWordsIntoPMWords = ""; 
        public string countWordsIntoEachLines; //количество слов в каждой строке
        public bool fileIsLoaded = false;
        
        spellCheсker(string[] args)
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
        }
    
        
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
            spellCheсker spellCheсker = new spellCheсker(args);
            if  (spellCheсker.fileIsLoaded)
            {
                string[] Arr_InputDictWords = spellCheсker.inputDictWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries);
                string[] Arr_inputPossibleMisspletWords = spellCheсker.inputPossibleMisspletWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries);
                string[] Arr_Result = new string[Arr_inputPossibleMisspletWords.Length];
                int ind_arr_result = 0;
                int current_word_print = 0;
                foreach (string possibleMisspletWord in Arr_inputPossibleMisspletWords)
                {
                    int srt_compare = 0;
                    string result_words = "";
                    int result_edits = -1;
                    srt_compare = 0;
                    foreach (string dictWord in Arr_InputDictWords)
                        {
                            if (srt_compare == 1)
                            {
                                continue;
                            }
                            if (String.Compare(possibleMisspletWord,dictWord,true)==0) 
                            {
                                srt_compare = 1;
                                result_edits = 0;
                                result_words = dictWord;
                            }
                            int levinstainDistance = 2;
                            if (((possibleMisspletWord.Length - possibleMisspletWord.Length)<=levinstainDistance) & ((possibleMisspletWord.Length - possibleMisspletWord.Length)>=-levinstainDistance)) 
                            { 
                                MethodLevenstain.possibleMisspletWord = possibleMisspletWord;
                                MethodLevenstain.dictWord = dictWord;
                                MethodLevenstain.isResolveFound = false;
                                MethodLevenstain.constrainTwoRedactionAdjactive = MethodLevenstain.notAllowConstrainTwoRedactionAdjactive;
                                MethodLevenstain.constrainMaxCountOfRedaction = 2;
                                MethodLevenstain.printOnlyMinOfWithRedaction = !MethodLevenstain.PrintOnlyMinOfWithRedaction;
                                MethodLevenstain.CompareTwoStrings();

                                if (MethodLevenstain.isResolveFound)    
                                {
                                    if (result_edits == -1)
                                    {
                                        result_edits = MethodLevenstain.minOfWithRedaction;
                                        result_words = dictWord;
                                    }
                                    else
                                        if (result_edits > MethodLevenstain.minOfWithRedaction)
                                        {
                                            result_edits = MethodLevenstain.minOfWithRedaction;
                                            result_words = dictWord;
                                        }
                                        else
                                            if (result_edits == MethodLevenstain.minOfWithRedaction)
                                            {
                                                result_words = result_words + " " + dictWord;
                                            }
                                }        
                            }   
                        }
                        if (result_edits ==  -1)
                            result_words = @"{"+possibleMisspletWord+"?}";
                        if (result_words.IndexOf(" ") > 0)
                            result_words = @"{"+result_words+"}";
                        // forming results
                        current_word_print++;
                    //printing results    
                    //    String repeatedString = new String('.', 10-possibleMisspletWord.Length);
                    //    String repeatedString1 = new String('.', 50-possibleMisspletWord.Length-result_words.Length);
                    //    Console.Write("PMWord: {1} {3} results: {0} {4} With words {2}",result_words,possibleMisspletWord,result_edits,repeatedString,repeatedString1);
                    //    Console.WriteLine();
                        Arr_Result[ind_arr_result] = result_words;
                        ind_arr_result++;
                    }

                    //printing results    
                    //    for(int i = 0; i<Arr_inputPossibleMisspletWords.Length;i++)
                    //    {
                    //        String repeatedString = new String('.', 10-Arr_inputPossibleMisspletWords[i].Length);
                    //        Console.WriteLine("Input: {0} {2} Output: {1}",Arr_inputPossibleMisspletWords[i],Arr_Result[i],repeatedString);
                    //    }
                spellCheсker.WriteResults(Arr_Result);

            }
        }
        catch (Exception ex)
            {
                Console.WriteLine("Исключение: {0}",ex.Message);
                Console.WriteLine("Метод: {0}",ex.TargetSite);
                Console.WriteLine("Трассировка стека: {0}",ex.StackTrace);
            }
    }
    }    
    }
}




                    