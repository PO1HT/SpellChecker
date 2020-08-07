using System;
using System.Collections.Generic;

namespace Redaction
{
    public static class MethodLevenstain
    {
        public const  int notInstalledConstrainMaxCountOfRedaction = -1;
        public const bool notAllowConstrainTwoRedactionAdjactive = true;
        public const bool PrintOnlyMinOfWithRedaction = true;
        public static int[,,] editorialDistance_;
        public static bool isTwoWordsEqual;
        public static string possibleMisspletWord;
        public static string dictWord;
        public static bool constrainTwoRedactionAdjactive = !notAllowConstrainTwoRedactionAdjactive;
        public static int constrainMaxCountOfRedaction = notInstalledConstrainMaxCountOfRedaction;
        public static HashSet<int> variantsLengthOfRedactionDistance;
        public static int counsOfEditorOrderTranslation;
        public static int minOfWithRedaction;  
        public static int levenstaneDistance;  
        public static bool printOnlyMinOfWithRedaction = !PrintOnlyMinOfWithRedaction;
        public static bool isResolveFound = !true;
        public static void CompareTwoStrings()
        {
            if (String.Compare(possibleMisspletWord,dictWord,true) == 0)  
                isTwoWordsEqual = true;
            else
                {
                    isTwoWordsEqual = false;
                    CalculateMatrix();
                    CountRedactions();
                    PrintOfVariandWords();
                }
        }

        public static void CalculateMatrix()
        {
            editorialDistance_ = new int[possibleMisspletWord.Length+1, dictWord.Length+1,4];              
            const int LayerEditorialDistance = 0;
            const int LayerArrowLeft = 1;
            const int LayerArrowUp = 2;
            const int LayerArrowDiagonalLeft = 3;
            const int firstColumn= 0;
            const int firstRow =  0;
            const int arrowUp = 1;
            const int arrowLeft = 1;
                
            for (int rowFirst = 1; rowFirst <= possibleMisspletWord.Length;rowFirst++)   
            {
                editorialDistance_[rowFirst,firstColumn,LayerEditorialDistance] = rowFirst;
                editorialDistance_[rowFirst,firstColumn,LayerArrowUp] = arrowUp; 
            }
            for (int columnFirst = 1; columnFirst <= dictWord.Length;columnFirst++)    
            {
                editorialDistance_[firstRow,columnFirst,LayerEditorialDistance] = columnFirst;
                editorialDistance_[firstRow,columnFirst,LayerArrowLeft] = arrowLeft;
            }
            for (int row=1;row <= possibleMisspletWord.Length;row++)
            {
                for (int column = 1;column <= dictWord.Length;column++)
                {
                    int arrowLeftInsertion = editorialDistance_[row,column-1,LayerEditorialDistance]+1; 
                    int arrowUpDelition = editorialDistance_[row-1,column,LayerEditorialDistance]+1; 
                    //!!!!!!! too long strng
                    int arrowDiagonalLeftReplacememtOrMatch = editorialDistance_[row-1,column-1,LayerEditorialDistance]+(((possibleMisspletWord[row-1] == dictWord[column-1]) ^ ((possibleMisspletWord[row-1]-32) == dictWord[column-1]) ^ (possibleMisspletWord[row-1] == (dictWord[column-1]-32)))?0:1); 
                    int recurrenceRatioOfEditorialDistance = Math.Min(Math.Min(arrowLeftInsertion,arrowUpDelition),arrowDiagonalLeftReplacememtOrMatch);
                    editorialDistance_[row,column,LayerEditorialDistance] = recurrenceRatioOfEditorialDistance;
                    int[] arrows = new int[4] {recurrenceRatioOfEditorialDistance,arrowLeftInsertion, arrowUpDelition, arrowDiagonalLeftReplacememtOrMatch};
                    for (int directionLayer = 1; directionLayer <= LayerArrowDiagonalLeft; directionLayer++) 
                        if (arrows[directionLayer] == recurrenceRatioOfEditorialDistance)
                            editorialDistance_[row,column,directionLayer] = 1;
                }    
            }
            
        }
        
    
        public static void PrintMatrixGorizontal(int directionStart,int directionEnd)
        {
            string[] dest_matrix = new string[] 
            {
                "MATRIX ==-D==-",
                "  LEFT   ARROW",
                "    UP   ARROW",
                "DiagLU   ARROW",
            };  
            Console.WriteLine("Gorizontal Print Editional Graph (Matrix)");
            for (int i = directionStart;i<=directionEnd;i++)
            {
                Console.Write("{0}{1}",dest_matrix[i], new string('_',(editorialDistance_.GetLength(1)-1)+15-dest_matrix[i].Length));
            }
            Console.WriteLine();
            for (int row = 1; row < ((editorialDistance_.GetLength(1)-1)+15)*(directionEnd-directionStart+1);row++)
                Console.Write("_");
            Console.WriteLine();
            for (int deep=directionStart;deep<=directionEnd;deep++)
                {
                    Console.Write("  |");
                    for (int column_ = 0;column_ <= editorialDistance_.GetLength(1)-1;column_++)
                    {
                        Console.Write("{0} ",column_);
                    }
                    Console.Write("  |");
                }
            Console.WriteLine();
            for (int row = 1; row < ((editorialDistance_.GetLength(1)-1)+15)*(directionEnd-directionStart+1);row++)
                Console.Write("-");
            Console.WriteLine();
                
            for (int row_ = 0;row_ <= editorialDistance_.GetLength(0)-1;row_++)
            {
                for (int deep=directionStart;deep<=directionEnd;deep++)
                {
                    Console.Write("{0} |",row_);
                    for (int column_ = 0;column_ <= editorialDistance_.GetLength(1)-1;column_++)
                    {
                        Console.Write("{0} ",editorialDistance_[row_,column_,deep]);
                    }
                    Console.Write("  |");
                }
                Console.WriteLine();
            }
        }
 
        public static void CountRedactions()
        {
            int[,,] editorialDistance = new int[possibleMisspletWord.Length+1, dictWord.Length+1,4];   
            for (int row = 0;row<=editorialDistance.GetLength(0)-1;row++)           
                for (int column = 0;column<=editorialDistance.GetLength(1)-1;column++)
                    for (int deep = 0;deep<=editorialDistance.GetLength(2)-1;deep++)
                        editorialDistance[row,column,deep] = MethodLevenstain.editorialDistance_[row,column,deep];   
            string S1_result = "";
            string S2_result = "";
            bool isLastForkFound = true;
            int rememberRow = 0;
            int rememberCloumn = 0;
            int rememberDirectionFork = 0;
            int count_variants = 0;
            int direction_ = 1;
            int row__ = editorialDistance.GetLength(0)-1;
            int column__ = editorialDistance.GetLength(1)-1;
            int countOfRedaction = 0;
            int lenOfRedaction = 0;
                                                                                
            while (isLastForkFound)
            {
                S1_result = "";
                S2_result = "";
                count_variants++;
                lenOfRedaction=0;
                countOfRedaction = 0;
                isLastForkFound = false;
                while ((row__ >= 0) & (column__ >= 0))
                {
                    direction_ = 1;
                    int sumForks = 0;
                    while ((editorialDistance[row__,column__,direction_] != 1) & (direction_<3))
                        direction_++;
                    for(int jDirection = direction_;jDirection <= 3;jDirection++)
                        sumForks = sumForks + editorialDistance[row__,column__,jDirection];
                    if ((sumForks > 1))
                    {
                        rememberRow = row__;
                        rememberCloumn = column__;
                        rememberDirectionFork = direction_;
                        isLastForkFound = true;
                    }
                    
                    if (direction_ == 1) 
                    {    
                        column__--;
                        countOfRedaction++;
                        lenOfRedaction++;
                    }
                    else
                        if (direction_ == 2)
                        {
                            row__--;
                            countOfRedaction++;
                            lenOfRedaction++;
                        }
                        else
                            if ((row__ != 0) & (column__ != 0))   // !!!!!!!!!!!!!!!!вот это мне совсем не нравится!!!!!!!!!!!
                            {
                                if (direction_ == 3) 
                                {
                                    if (possibleMisspletWord[row__-1] == dictWord[column__-1]) //Match
                                    {
                                        lenOfRedaction++;
                                    }
                                    else
                                    {       
                                        S1_result = possibleMisspletWord[row__-1] + S1_result;
                                        S2_result = dictWord[column__-1] + S2_result;
                                        countOfRedaction = countOfRedaction + 2;
                                        lenOfRedaction++;
                                    }
                                row__--;
                                column__--;
                                }
                            }
                            else
                            {
                                row__--;
                                column__--;
                            }
                }
                editorialDistance[rememberRow,rememberCloumn,rememberDirectionFork] = 0;
                row__ = possibleMisspletWord.Length;
                column__ = dictWord.Length;
                if (count_variants == 1)
                {
                     minOfWithRedaction  = countOfRedaction;//lenOfRedaction;  
                }
                else
                {
                    if (minOfWithRedaction > countOfRedaction)//lenOfRedaction)
                        minOfWithRedaction = countOfRedaction;//lenOfRedaction;
                }

                
            }
            counsOfEditorOrderTranslation = count_variants;
        }

        public static int[] AllRedactions(int countOfVariants)
        {
            int[,,] editorialDistance = new int[possibleMisspletWord.Length+1, dictWord.Length+1,4];   
            for (int row = 0;row<=editorialDistance.GetLength(0)-1;row++)           
                for (int column = 0;column<=editorialDistance.GetLength(1)-1;column++)
                    for (int deep = 0;deep<=editorialDistance.GetLength(2)-1;deep++)
                        editorialDistance[row,column,deep] = editorialDistance_[row,column,deep];   
            int[] arr = new int[countOfVariants];
            string S1_result = "";
            string S2_result = "";
            bool isLastForkFound = true;
            int rememberRow = 0;
            int rememberCloumn = 0;
            int rememberDirectionFork = 0;
            int direction_ = 1;
            int row__ = editorialDistance.GetLength(0)-1;
            int column__ = editorialDistance.GetLength(1)-1;
            int countOfRedaction = 0;
            int count_variants = 0;                                      
            while (isLastForkFound)
            {
                S1_result = "";
                S2_result = "";
                countOfRedaction = 0;
                isLastForkFound = false;
                while ((row__ >= 0) & (column__ >= 0))
                {
                    direction_ = 1;
                    int sumForks = 0;
                    while ((editorialDistance[row__,column__,direction_] != 1) & (direction_<3))
                        direction_++;
                    for(int jDirection = direction_;jDirection <= 3;jDirection++)
                        sumForks = sumForks + editorialDistance[row__,column__,jDirection];
                    if ((sumForks > 1))
                    {
                        isLastForkFound = true;
                        rememberRow = row__;
                        rememberCloumn = column__;
                        rememberDirectionFork = direction_;
                        isLastForkFound = true;
                    }
                    if (direction_ == 1) 
                    {
                        countOfRedaction++;
                        column__--;
                    }
                    else
                        if (direction_ == 2)
                        {
                            countOfRedaction++;
                            row__--;
                        }
                        else
                            if ((row__ != 0) & (column__ != 0))   // !!!!!!!!!!!!!!!!вот это мне совсем не нравится!!!!!!!!!!!
                            {
                                if (direction_ == 3) 
                                {
                                    if (possibleMisspletWord[row__-1] == dictWord[column__-1]) //Match
                                    {
                                        
                                    }
                                    else
                                    {       
                                        S1_result = possibleMisspletWord[row__-1] + S1_result;
                                        S2_result = dictWord[column__-1] + S2_result;
                                        countOfRedaction = countOfRedaction + 2;
                                    }
                                row__--;
                                column__--;
                                }
                            }
                            else
                            {
                                row__--;
                                column__--;
                            }
                }
                editorialDistance[rememberRow,rememberCloumn,rememberDirectionFork] = 0;
                row__ = possibleMisspletWord.Length;
                column__ = dictWord.Length;
                arr[count_variants] = countOfRedaction;
                count_variants++;
            }
            return arr;
        }

        public static void PrintOfVariandWords()
        {
            int[,,] editorialDistance = new int[possibleMisspletWord.Length+1, dictWord.Length+1,4];   
            for (int row = 0;row<=editorialDistance.GetLength(0)-1;row++)           
                for (int column = 0;column<=editorialDistance.GetLength(1)-1;column++)
                    for (int deep = 0;deep<=editorialDistance.GetLength(2)-1;deep++)
                        editorialDistance[row,column,deep] = MethodLevenstain.editorialDistance_[row,column,deep];            
            string S1_result = "";
            string S2_result = "";
            bool isLastForkFound = true;
            int rememberRow = 0;
            int rememberCloumn = 0;
            int rememberDirectionFork = 0;
            int direction_ = 1;
            int row__ = possibleMisspletWord.Length;
            int column__ = dictWord.Length;
            int countOfRedaction = 0;
            bool resolveFounded = true;
            int  lenOfRedaction = 0;
            while (isLastForkFound)
            {
                int previousDirection_ = -1;
                S1_result = "";
                S2_result = "";
                countOfRedaction = 0;
                lenOfRedaction = 0;
                isLastForkFound = false;
                resolveFounded = true;
                while (((row__ >= 0) & (column__ >= 0)) & (resolveFounded))
                {
                    
                    int sumForks = 0;
                    direction_ = 1;
                    while ((editorialDistance[row__,column__,direction_] == 0) & (direction_<3))
                    {
                        direction_++;
                    }
                    for(int jDirection = direction_;jDirection <= 3;jDirection++)
                    {
                        sumForks = sumForks + editorialDistance[row__,column__,jDirection];
                    }
                    if (sumForks > 1)
                    {
                        rememberRow = row__;
                        rememberCloumn = column__;
                        rememberDirectionFork = direction_;
                        isLastForkFound = true;
                    }
                    if (direction_ == 1) 
                    {
                        S1_result = "_" + S1_result;
                        S2_result = dictWord[column__-1] + S2_result;
                        column__--;
                        countOfRedaction++;
                        lenOfRedaction++;
                    }
                                                
                    if (direction_ == 2)
                    {
                        S1_result = possibleMisspletWord[row__-1] + S1_result;
                        S2_result = "_" + S2_result;
                        row__--;
                        countOfRedaction++;
                        lenOfRedaction++;
                    }
                    if ((row__ != 0) & (column__ != 0))   // !!!!!!!!!!!!!!!!вот это мне совсем не нравится!!!!!!!!!!!
                    {
                        if (direction_ == 3) 
                        {
                            if (possibleMisspletWord[row__-1] == dictWord[column__-1]) //Match
                            {
                                S1_result = possibleMisspletWord[row__-1] + S1_result;
                                S2_result = dictWord[column__-1] + S2_result;
                                lenOfRedaction++;
                            }
                        else //replacement
                        {
                            S1_result = possibleMisspletWord[row__-1] + S1_result;
                            S2_result = dictWord[column__-1] + S2_result;
                            countOfRedaction = countOfRedaction + 2;
                            lenOfRedaction++;
                        }
                        row__--;
                        column__--;
                        }
                    }
                    else
                    {
                        row__--;
                        column__--;
                    }
                    if (((constrainTwoRedactionAdjactive) & (direction_ != 3) & (previousDirection_ == direction_)) & (constrainTwoRedactionAdjactive))
                    {
                        resolveFounded = false;
                    }
                    previousDirection_ = direction_;
                    if ((constrainMaxCountOfRedaction < countOfRedaction) & (constrainMaxCountOfRedaction != notInstalledConstrainMaxCountOfRedaction)) 
                    {
                        resolveFounded = false;
                    }
                    if ((printOnlyMinOfWithRedaction) && (minOfWithRedaction < lenOfRedaction))
                    {
                        resolveFounded = false;
                    }
                }
                if (resolveFounded)
                {
                    isResolveFound = true;

/*                    count_variants++;
                    Console.WriteLine("Variant: {0}",count_variants);
                    Console.WriteLine("========================================================================================");
                    Console.WriteLine();
                    Console.WriteLine("    {0}",S2_result);
                    Console.WriteLine("    {0}",S1_result);
                    Console.WriteLine();
                    Console.WriteLine("Number of redactions: {0}",countOfRedaction);
                    Console.WriteLine("________________________________________________________________________________________");
  */
                }                        
                editorialDistance[rememberRow,rememberCloumn,rememberDirectionFork] = 0;
                row__ = possibleMisspletWord.Length;
                column__ = dictWord.Length;
            }
                
        }
    

    public static void MethodValenstaintTest()
        {
            try
            {
                //original Lines
                MethodLevenstain.possibleMisspletWord = "the";
                MethodLevenstain.dictWord = "hte";
                //Method of calculate editorial Distance 
                MethodLevenstain.CompareTwoStrings();
                //Found results. Parametrs.
                MethodLevenstain.constrainTwoRedactionAdjactive = !MethodLevenstain.notAllowConstrainTwoRedactionAdjactive;
                MethodLevenstain.constrainMaxCountOfRedaction = MethodLevenstain.notInstalledConstrainMaxCountOfRedaction;
                MethodLevenstain.printOnlyMinOfWithRedaction = !MethodLevenstain.PrintOnlyMinOfWithRedaction;
                
                //Method of print result 
                MethodLevenstain.PrintOfVariandWords();
                //Method of print matrix of Distanse (Edition Graph)
                MethodLevenstain.PrintMatrixGorizontal(0,3);

                //Count of founded variants
                int countOfVariants = MethodLevenstain.counsOfEditorOrderTranslation ;
                Console.WriteLine("countOfVariants {0}",countOfVariants);
                //Count of found minimal Levinstain distance
                Console.WriteLine("minOfWithRedaction {0}",MethodLevenstain.minOfWithRedaction);
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
        
    

                