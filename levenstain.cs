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
        public static string possibleMisspletWord;
        public static string dictWord;
        public static bool constrainTwoRedactionAdjactive = !notAllowConstrainTwoRedactionAdjactive;
        public static int constrainMaxCountOfRedaction = notInstalledConstrainMaxCountOfRedaction;
        public static int counsOfEditorOrderTranslation;
        public static int minOfWithRedaction;  
        public static int levenstaneDistance;  
        public static bool printOnlyMinOfWithRedaction = !PrintOnlyMinOfWithRedaction;
        public static bool isResolveFound = !true;
        public static bool caseSensitivity = true;

        public static void SetCaseSensitivity(bool flag)
        {
            caseSensitivity = flag;
        }
        
        public static bool isTwoWordsEqual 
        {
            get 
            {
                if (String.Compare(possibleMisspletWord,dictWord,caseSensitivity) == 0)  
                    return  true;
                else
                    return  false;
            }
            set 
            {

            }
        }
                
        public static void CompareTwoStrings()
        {
            if (!isTwoWordsEqual)
            {
                CalculateMatrix();
                CountRedactions();
                PrintOfVariandWords();
            }
        }

        enum Layer : int
            {
                EditorialDistance = 0,
                ArrowLeft = 1,
                ArrowUp = 2,
                ArrowDiagonalLeft = 3,
            }
            
        const int equalsMinValueStep = 1;

        enum Arrow : int
        {
            Up = equalsMinValueStep,
            Left = equalsMinValueStep,
            DiagonalLeft = equalsMinValueStep,
        }
        enum First : int
        {
            Column = 0,
            Row = 0
        }

        public static void MatrixInit()
        {
            int rowFirst = (int)First.Row;
            int columnFirst = (int)First.Column;
            
            editorialDistance_ = new int[possibleMisspletWord.Length+1, dictWord.Length+1,4];              
            
            editorialDistance_[rowFirst,columnFirst,(int)Layer.EditorialDistance] = 0;
            editorialDistance_[rowFirst,columnFirst,(int)Layer.ArrowUp] = 0;
            editorialDistance_[rowFirst,columnFirst,(int)Layer.ArrowLeft] = 0;
            editorialDistance_[rowFirst,columnFirst,(int)Layer.ArrowDiagonalLeft] = 0;
            
            for (int i = 1; (i <= Math.Max(editorialDistance_.GetLength(0),editorialDistance_.GetLength(1))-1);i++)   
            {
                if (i <= (editorialDistance_.GetLength(0)-1))
                {
                    rowFirst = i;
                    editorialDistance_[rowFirst,(int)First.Column,(int)Layer.EditorialDistance] = rowFirst;
                    editorialDistance_[rowFirst,(int)First.Column,(int)Layer.ArrowUp] = (int)Arrow.Up; 
                }
                if (i <= (editorialDistance_.GetLength(1)-1))
                {
                    columnFirst = i;
                    editorialDistance_[(int)First.Row,columnFirst,(int)Layer.EditorialDistance] = columnFirst;
                    editorialDistance_[(int)First.Row,columnFirst,(int)Layer.ArrowLeft] = (int)Arrow.Left;
                }    
            }   
        }

        public static int minOfTreeValues(int arrowLeftInsertion,int arrowUpDelition, int arrowDiagonalLeftReplacememtOrMatch)
        {
            return Math.Min(Math.Min(arrowLeftInsertion,arrowUpDelition),arrowDiagonalLeftReplacememtOrMatch);
        }
        
        public static void FillMainLayerOfMatrix(int row, int column)
        {
                    editorialDistance_[row,column,(int)Layer.EditorialDistance] = minOfTreeValues(
                        
                        editorialDistance_[row,column-1,(int)Layer.EditorialDistance]+1,
                        editorialDistance_[row-1,column,(int)Layer.EditorialDistance]+1,
                        editorialDistance_[row-1,column-1,(int)Layer.EditorialDistance]+
                                                (
                                                    (
                                                        (possibleMisspletWord[row-1] == dictWord[column-1]) ^ 
                                                        ((possibleMisspletWord[row-1]-32) == dictWord[column-1]) ^ 
                                                        (possibleMisspletWord[row-1] == (dictWord[column-1]-32))
                                                    )
                                                        ?0:1
                                                )
                        );
        }
        
        public static void FillMatrixDirectionsOfMinimalWays(int row, int column)
        {
            int arrowLeftInsertion = editorialDistance_[row,column-1,(int)Layer.EditorialDistance]+1; 
            int arrowUpDelition = editorialDistance_[row-1,column,(int)Layer.EditorialDistance]+1; 
            int tCompareSymbolsOfCureentPosition = (((possibleMisspletWord[row-1] == dictWord[column-1]) ^ ((possibleMisspletWord[row-1]-32) == dictWord[column-1]) ^ (possibleMisspletWord[row-1] == (dictWord[column-1]-32)))?0:1);
            int arrowDiagonalLeftReplacememtOrMatch = editorialDistance_[row-1,column-1,(int)Layer.EditorialDistance]+tCompareSymbolsOfCureentPosition; 
            int[] arrows = new int[4] {editorialDistance_[row,column,(int)Layer.EditorialDistance], arrowLeftInsertion,arrowUpDelition,arrowDiagonalLeftReplacememtOrMatch};
            for (int directionLayer = 1; directionLayer <= (int)Layer.ArrowDiagonalLeft; directionLayer++) 
                if (arrows[directionLayer] == editorialDistance_[row,column,(int)Layer.EditorialDistance])
                    editorialDistance_[row,column,directionLayer] = equalsMinValueStep;
        }

        public static void CalculateMatrix()
        {
            MatrixInit();
            for (int row=1;row <= possibleMisspletWord.Length;row++)
                for (int column = 1;column <= dictWord.Length;column++)
                    {
                        FillMainLayerOfMatrix(row, column);
                        FillMatrixDirectionsOfMinimalWays(row,column);
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
#warning  this block need changes;
                            if ((row__ != 0) & (column__ != 0))  
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
                     minOfWithRedaction  = countOfRedaction;
                }
                else
                {
                    if (minOfWithRedaction > countOfRedaction)
                        minOfWithRedaction = countOfRedaction;
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
                            if ((row__ != 0) & (column__ != 0))   
                            {
                                if (direction_ == 3) 
                                {
                                    if (possibleMisspletWord[row__-1] == dictWord[column__-1]) 
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
#warning  this block need changes;
                    if ((row__ != 0) & (column__ != 0))   
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
        
    

                