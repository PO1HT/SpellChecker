using System;
using System.IO;
using System.Collections.Generic;
using WorkArgs;

using System.Diagnostics;

namespace SpellCheckerIO
{
    class SpellCheсker
    {
        private string inputFullFileName;
        private string otputFullFileName;
        private string inputDictWords = "";
        private string inputPossibleMisspletWords = ""; 
        private string countWordsIntoDictWords = ""; 
        private string countWordsIntoPMWords = ""; 
        private string countWordsIntoEachLines;
        private bool fileIsLoaded = false;
        private bool fileOpenAfterWrite = false;

        public bool FileIsLoaded()
        {
            return fileIsLoaded;
        }
        public string GetInputDictWords()
        {
            return inputDictWords;
        }

        public string GetInputPossibleMisspletWords()
        {
            return inputPossibleMisspletWords;
        }
        public void FileOpenAfterWriteOn()
        {
            fileOpenAfterWrite = true;
        }
        
        public void SetFileOpenAfterWriteOff()
        {
            fileOpenAfterWrite = false;
        }
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
                ReadingSourceFile();
            else
                throw new Exception("Input File is does't exist. Please check and fix the filename and try again! See soon");
        }

        public bool IsNotZeroLengthDestanationString(string destanation)
        {
            if (destanation.Length != 0)
                return true;
            else   
                return false;
        }

        public bool IsLastSymbolSpase(string destanation)
        {
            if (destanation[destanation.Length-1] == ' ')
                return true;
            else   
                return false;
        }

        public string AddDestanashionWordWithoutDobleTrim(string destanation, string newPMWord)
        {
            if (IsLastSymbolSpase(destanation))
                return destanation +  newPMWord;
            else    
                return destanation + ' ' + newPMWord;
        }

        public string AddInputWord(string destanation, string newPMWord)
        {
            if (IsNotZeroLengthDestanationString(destanation))
                return AddDestanashionWordWithoutDobleTrim(destanation, newPMWord);
            else
                return newPMWord;
        }
        
        public bool IsNotSepparator(string match)
        {
            string sepparator = "===";
            if (String.Compare(match.Trim(), sepparator) != 0)
                return true;
            else
                return false;
        }
        
        public void AddCountWordsIntoCurrentLine(string[] words)
        {
            countWordsIntoEachLines = countWordsIntoEachLines + words.Length.ToString();
        }

        public string AddWordsIntoDestanation(string destanation, string[] words)
        {
            foreach(string currentWord in words)
                destanation = AddInputWord(destanation, currentWord);
            return destanation;
        }

        public string AddSourceLineToDestanation(string destanation, string line)
        {
            string[] words =  line.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries);
            AddCountWordsIntoCurrentLine(words);
            return AddWordsIntoDestanation(destanation, words);
        }
        
        public void SetNothingInCountWordsIntoEachLines()
        {
            countWordsIntoEachLines = "";
        } 
            
        public bool IsNotEOFTheEndOfFile(StreamReader inputFile)
        {
            const bool notEndOfFile = false;
            return (inputFile.EndOfStream == notEndOfFile);
        }

        public String ReadPartFile(StreamReader inputFile)
        {
            SetNothingInCountWordsIntoEachLines();
            String destanation = "";
            String line;
            while ((IsNotEOFTheEndOfFile(inputFile)) && (IsNotSepparator(line = inputFile.ReadLine())))
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

        public void SetFileIsLoadedSuccess()
        {
            fileIsLoaded = true;
        }

        public void SetFileIsLoadedFail()
        {
            fileIsLoaded = false;
        }

        public void ReadingSourceFile()
        {
            try
                {
                    using (StreamReader inputFile = new StreamReader(@inputFullFileName))
                    {
                        ReadInputFile(inputFile);
                    }
                    SetFileIsLoadedSuccess();
                }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                SetFileIsLoadedFail();
            }  
        }

        public int[] EachLineCountOfWordsToArray(string[] result_words)
        {
            const int offsetOfNumbericChar = 48;
            int[] arr_r = new int[countWordsIntoPMWords.Length];
            int prev = 0;
            for (int i = 0;i < countWordsIntoPMWords.Length;i++)
            {
                arr_r[i] = prev + Convert.ToInt16(countWordsIntoPMWords[i]) - offsetOfNumbericChar;
                prev = arr_r[i];
            }
            return arr_r; 
        }

        public int TotalWordCount(int[] arr_r)
        {
            return arr_r[arr_r.Length-1];
        }

        public bool IncIfTheEndOfLine(int numberOfCurrentPrintWord, int numberOfTheEndWord)
        {
            if (numberOfCurrentPrintWord == numberOfTheEndWord)
                return true;
            return false;
        }

        public string ResultIntoString(string[] result_words)
        {
            int[] arr_r = EachLineCountOfWordsToArray(result_words);
            string result = "";
            for (int i = 0;i<arr_r.Length;i++)
            {
                for (int j = ((i==0)?0:arr_r[i-1]);j<arr_r[i];j++)
                    result = result + result_words[j] + " ";
                result = result + "\u000D";
            }
            return result;
        }
    
        public void WriteResults(string[] result_words)
        {
            using (StreamWriter Outf = new StreamWriter(otputFullFileName))
            {
                Outf.Write(ResultIntoString(result_words));
                OpenNotepadResults();
            }
        }
        
        public void OpenNotepadResults()
        {
            if (fileOpenAfterWrite) Process.Start("notepad.exe", otputFullFileName);
        }
    }
}