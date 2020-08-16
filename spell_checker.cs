using System;
using System.IO;
using System.Collections.Generic;
using RedactionGraph;
using WorkArgs;
using SpellCheckerIO;

namespace TwoEdits
{
    public class Prepare
    {
        private string[] Arr_Result;
        private string[] Arr_InputDictWords;
        private string[] Arr_InputPossibleMisspletWords;
        private bool caseSensitivity = true;

        public string[] GetArrResult()
        {
            return Arr_Result;
        }
        public Prepare(string sourceDictWords,string sourcePossibleMisspletWords)
        {
            SetArr_InputDictWords(sourceDictWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries));
            SetArr_InputPossibleMisspletWords(sourcePossibleMisspletWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries));
        }
        public void SetCaseSensitivity(bool flag)
        {
            caseSensitivity = flag;
        }

        public void SetArr_InputDictWords(string[] source)
        {
            Arr_InputDictWords = source;
        }
                
        public void SetArr_InputPossibleMisspletWords(string[] source)
        {
            Arr_InputPossibleMisspletWords = source;
        }
        private int ind_arr_result = 0;
        
        public void CreateResults()
        {
            ind_arr_result = 0;
            this.Arr_Result = new string[this.Arr_InputPossibleMisspletWords.Length];
            this.PrepareResults(this.Arr_InputPossibleMisspletWords,this.Arr_InputDictWords);
        }

        private bool FindRedactionEditsPossibleMisspleWordIntoDictWord(string possibleMisspletWord, string dictWord)
        {
            MethodLevenstain.SetPossibleMisspletWord(possibleMisspletWord);
            MethodLevenstain.SetDictWord(dictWord);
            MethodLevenstain.NewSeek();
            MethodLevenstain.ConstrainTwoRedactionAdjactiveOn();
            MethodLevenstain.constrainMaxCountOfRedaction = 2;
            MethodLevenstain.PrintOnlyMinOfWithRedactionOn(); 
            MethodLevenstain.SetCaseSensitivity(true);
            MethodLevenstain.CompareTwoStrings();
            return MethodLevenstain.isWordFounded();
        }
        enum resultFound : int
            {
                notFoundEdits = -1,
                foundEdits = 0,
                notFoundAnyWords = 0,
                notStringMatch = 0,
                stringMatch = 1
            }
            
        public void PrepareResults(string[] arr_InputPossibleMisspletWords, string[] arr_InputDictWords)
        {
            int current_word_print = 0;
            foreach (string possibleMisspletWord in arr_InputPossibleMisspletWords)
            {
                string result_words = "";
                int minCountOfEdits = Int16.MaxValue;//(int)resultFound.notFoundEdits;
                foreach (string dictWord in arr_InputDictWords)
                {
                    if (String.Compare(possibleMisspletWord,dictWord,caseSensitivity)==0) 
                    {
                        result_words = dictWord;
                        break;
                    }
                    
                    int levinstainDistance = 2;
                    if (((possibleMisspletWord.Length - possibleMisspletWord.Length)<=levinstainDistance) & ((possibleMisspletWord.Length - possibleMisspletWord.Length)>=-levinstainDistance)) 
                    { 
                        if (FindRedactionEditsPossibleMisspleWordIntoDictWord(possibleMisspletWord,dictWord))    
                        {
                            if (result_words.Length == 0)
                            {
                                minCountOfEdits = MethodLevenstain.minOfWithRedaction;
                                result_words = dictWord;
                            }
                            else
                                if (minCountOfEdits > MethodLevenstain.minOfWithRedaction)
                                {
                                    minCountOfEdits = MethodLevenstain.minOfWithRedaction;
                                    result_words = dictWord;
                                }
                                else
                                    if (minCountOfEdits == MethodLevenstain.minOfWithRedaction)
                                    {
                                        result_words = result_words + " " + dictWord;
                                    }
                        }        
                    }   
                }
                
                if (result_words.Length == 0)
                    result_words = @"{"+possibleMisspletWord+"?}";
                
                if (result_words.IndexOf(" ") > (int)resultFound.notFoundAnyWords)
                    result_words = @"{"+result_words+"}";
                
                current_word_print++;
                
                AddWordIntoResult(result_words);
                
            }
        }

        private void AddWordIntoResult(string words)
        {
            Arr_Result[ind_arr_result] = words;
            ind_arr_result++;
        }

    }
    
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
            SpellCheckerIO.SpellCheсker spellCheсker = new SpellCheckerIO.SpellCheсker(args);
            if  (spellCheсker.FileIsLoaded())
            {
                Prepare FinderRedactionWord = new Prepare(
                                                            spellCheсker.GetInputDictWords(),
                                                            spellCheсker.GetInputPossibleMisspletWords()
                                                         );
                FinderRedactionWord.CreateResults();
                
                spellCheсker.FileOpenAfterWriteOn(); 
                spellCheсker.WriteResults(FinderRedactionWord.GetArrResult());
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




                    