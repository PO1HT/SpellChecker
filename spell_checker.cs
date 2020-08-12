using System;
using System.IO;
using System.Collections.Generic;
using Redaction;
using WorkArgs;
using SpellCheckerIO;

namespace TwoEdits
{
    public class Prepare
    {
        public static string[] Arr_Result;
        public static string[] Arr_InputDictWords;
        public static string[] Arr_inputPossibleMisspletWords;
        public static bool caseSensitivity = true;

        public static void SetCaseSensitivity(bool flag)
        {
            caseSensitivity = flag;
        }

        public static void SetArr_InputDictWords(string[] source)
        {
            Arr_InputDictWords = source;
        }
                
        public static void SetArr_inputPossibleMisspletWords(string[] source)
        {
            Arr_inputPossibleMisspletWords = source;
        }
        public static void CreateResults()
        {
            TwoEdits.Prepare.Arr_Result = new string[Arr_inputPossibleMisspletWords.Length];
            TwoEdits.Prepare.PrepareResults(Arr_inputPossibleMisspletWords,Arr_InputDictWords);
        }
        public static void PrepareResults(string[] Arr_inputPossibleMisspletWords, string[] Arr_InputDictWords)
        {
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
                    if (String.Compare(possibleMisspletWord,dictWord,caseSensitivity)==0) 
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
                        MethodLevenstain.SetCaseSensitivity(true);
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
                current_word_print++;
                Arr_Result[ind_arr_result] = result_words;
                ind_arr_result++;
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
            SpellCheckerIO.SpellCheсker spellCheсker = new SpellCheckerIO.SpellCheсker(args);
            if  (spellCheсker.fileIsLoaded)
            {
                Prepare.SetArr_InputDictWords(spellCheсker.inputDictWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries));
                Prepare.SetArr_inputPossibleMisspletWords(spellCheсker.inputPossibleMisspletWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries));
                Prepare.CreateResults();
                spellCheсker.SetFileOpenAfterWrite(true); 
                spellCheсker.WriteResults(TwoEdits.Prepare.Arr_Result);
            }
            else
              throw new Exception("Input File is does't exist. Please check and fix the filename and try again! See soon");
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




                    