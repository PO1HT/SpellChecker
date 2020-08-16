using System;
using System.Collections.Generic;

namespace RedactionGraph
{
    public static class PrintMethodLevenstain
    {
    
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