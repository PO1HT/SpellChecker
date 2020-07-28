using System;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;


namespace Two_Edits
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + @"\";
            String InputFileName;
            String OutputFileName;
                
            if ((args.Length <= 2) & (args.Length != 0))
            {
                InputFileName = args[0];  //InputFileName =  Console.ReadLine();
            }
            else
            {
                InputFileName =  "Input.txt";
            }
            if ((args.Length == 2) & (args.Length != 0))
            {
                OutputFileName = args[1];  //InputFileName =  Console.ReadLine();
            }
            else
            {
                OutputFileName=  "Output.txt";
            }
                

            String InputDictWords = ""; //dictionary by a sequence of;
            String InputPMWords = ""; //Possible Misspelt Words;
            String InputPMWords_cnt ="";
            int current_word_print = 0;
            if (File.Exists(@InputFileName))
            {
                try
                {
                    using (StreamReader InputFile = new StreamReader(@InputFileName))
                    {
                        bool PartOfIncText = false; //Part 1 is false; Part 2 is true
                        string line;
                        string sepparator = "===";
                        while ((line = InputFile.ReadLine()) != null)
                        {
                            string[] array= line.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries);  
                            if (String.Compare(array[0], sepparator) == 0)
                            {
                                PartOfIncText = true;
                            } 
                            else
                            {
                                int cnt_words_line;
                                cnt_words_line = 0;
                                foreach(string x in array)
                                {
                                    cnt_words_line++;
                                    if (PartOfIncText)
                                    {
                                        if (InputPMWords.Length != 0)
                                        {   
                                            if (InputPMWords[InputPMWords.Length-1] == ' ' )
                                            {
                                                InputPMWords = InputPMWords +  x;
                                            }
                                            else    
                                            {
                                                InputPMWords = InputPMWords + ' ' + x;
                                            }
                                        }    
                                        else
                                            {
                                                InputPMWords = InputPMWords + x;
                                            }
                                        }
                                    else
                                    {
                                        if (InputDictWords.Length != 0)
                                        {   
                                            if (InputDictWords[InputDictWords.Length-1] == ' ' )
                                            {
                                                InputDictWords = InputDictWords +  x;
                                            }
                                            else    
                                            {
                                                InputDictWords = InputDictWords + ' ' + x;
                                            }
                                        }    
                                        else
                                        {
                                            InputDictWords = InputDictWords + x;
                                        }
                                    }
                                }
                                if (PartOfIncText)
                                {
                                    InputPMWords_cnt = InputPMWords_cnt + cnt_words_line.ToString();
                                }
                            }                                                
                            Array.Clear(array,0,array.Length); 
                        }
                    }
                }                
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }  
                StreamWriter Outf = new StreamWriter(OutputFileName);//,false,Encoding.GetEncoding("UTF-8"));
                string[] Arr_InputDictWords = InputDictWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries);
                string[] Arr_InputPMWords = InputPMWords.Split(new[] {' '},StringSplitOptions.RemoveEmptyEntries);
                int srt_compare;
                int compare_found;
                foreach (string S1 in Arr_InputPMWords)
                {
                    compare_found = 0;
                    srt_compare = 0;
                    HashSet<string> result_words_2edits = new HashSet<string>();
                    HashSet<string> result_words_1edits = new HashSet<string>();
                    string result_words_0edit = "";
                    foreach (string S2 in Arr_InputDictWords)
                        {
                        if (srt_compare == 1)
                            {
                                continue;
                            }
                        if (String.Compare(S1,S2,true)==0) 
                            {
                                srt_compare = 1;
                                compare_found = 1;
                                result_words_0edit = S2;
                            }
                        if (((S1.Length - S1.Length)<=2) & ((S1.Length - S1.Length)>=-2)) //Create matrix T if S1[i] == S2[j] then T[i,j] = 0; if S1[i] <> S2[j] then T[i,j] = 1;
                        { 
                            int[,] T = new int[S1.Length+1, S2.Length+1];  // Three layer of pointers in order: layer 1 is LeftArrow, Layer 2 is UpArrow, Layer 3 is DiagonalLeftArrow
                            for (int i = 0; i <= S1.Length-1; i++) // fill in all the elements of the array (1.. S1.length) and (1.. S2.length)
                            {
                                for (int j = 0; j <= S2.Length-1; j++)
                                {
                                if ((S1[i] == S2[j]) ^ ((S1[i]-32) == S2[j]) ^ (S1[i] == (S2[j]-32)))
                                    {
                                            T[i+1,j+1]=0;
                                    }
                                    else    
                                    {
                                            T[i+1,j+1]=1;
                                    }
                                }
                            }
                            //Create matrix for redactions graph                                        
                            int[,,] D = new int[S1.Length+1, S2.Length+1,6];  // Three layer of pointers in order: layer 0 is LeftArrow, Layer 1 is UpArrow, Layer 2 is DiagonalLeftArrow
                            for (int x = 0; x <= S1.Length;x++)    //fill the first row
                            {
                                D[x,0,0] = x;
                            }
                            for (int x = 0; x <= S2.Length;x++)    //and fill the first column
                            {
                                D[0,x,0] = x;
                            }
                            // to fill 0 - row and 0 column
                            //D[i,0] = i
                            //D[0,j] = j
                            for (int i = 1; i <= S1.Length; i++)
                            {
                                D[i,0,0] = i;
                                D[i,0,2] = 1;
                            }   
                            for (int j = 1; j <= S2.Length; j++)
                            {
                                D[0,j,0] = j;
                                D[0,j,1] = 1;
                            }
                            for (int i = 1; i <= S1.Length; i++)
                            {
                                for (int j = 1; j <= S2.Length; j++)
                                {
                                    int expr1 = D[i,j-1,0]+1; //1 - step left
                                    int expr2 = D[i-1,j,0]+1; //2 - step up
                                    int expr3 = D[i-1,j-1,0]+T[i,j]; //DiagonalLeftArrow
                                    int[] arrExpr =  new int[3];
                                    arrExpr[0] = expr1;
                                    arrExpr[1]= expr2;
                                    arrExpr[2] = expr3;
                                    Array.Sort(arrExpr);
                                    D[i,j,0] = arrExpr[0];
                                    if (arrExpr[0] == expr1)
                                    {
                                            D[i,j,1] = 1;
                                    }
                                    else    
                                    {
                                            D[i,j,1] = 0;
                                    }    
                                    if (arrExpr[0] == expr2)
                                    {
                                            D[i,j,2] = 1;
                                    }
                                    else    
                                    {
                                            D[i,j,2] = 0;
                                    }    
                                    if (arrExpr[0] == expr3)
                                    {
                                            D[i,j,3] = 1;
                                    }
                                    else    
                                    {
                                    D[i,j,3] = 0;
                                    }    
                                }    
                            }
                            string[] dest_matrix = new string[] 
                            {
                                "MATRIX ==-D==-",
                                "  LEFT   ARROW",
                                "    UP   ARROW",
                                "DiagLU   ARROW",
                                "   DEL  MATRIX",
                                "VARIANT MATRIX"
                            };
                            int count_variant = 1;
                            D[S1.Length,S2.Length,4] = 1;
                            for (int i = S1.Length; i >= 0; i--)
                            {
                                for (int j = S2.Length; j >= 0; j--)
                                {
                                    if (D[i,j,4] != 1)
                                    {
                                            D[i,j,0] = 0;
                                    }   
                                    else
                                    {
                                        if (D[i,j,1] == 1)
                                        {
                                            D[i,j-1,4] = 1; 
                                        }
                                        if (D[i,j,2] == 1)
                                        {
                                            D[i-1,j,4] = 1;
                                        }
                                        if (D[i,j,3] == 1)
                                        {
                                            D[i-1,j-1,4] = 1;
                                        }
                                        D[i,j,5] = D[i,j,1] + D[i,j,2] + D[i,j,3];
                                        if (D[i,j,5] > 1) 
                                        {
                                            count_variant = count_variant + D[i,j,5] - 1;
                                        }
                                    }    
                                }
                            }
                            string[] S1_result = new string[count_variant];
                            string[] S2_result  = new string[count_variant];
                            int i1, i_last, s1_cur,  s2_cur;
                            int j1, j_last;
                            int cnt_ins;
                            int cnt_del;
                            int cnt_rep;
                            int both_operation_adjacent_ins;
                            int both_operation_adjacent_del;
                            int both_operation_adjacent_skip;
                              //      int both_operation_adjacent_skip;
                        
                            for (int k = 0; k < count_variant;k++) //sycle for 
                            {
                                i1 = S1.Length;
                                j1 = S2.Length;
                                s1_cur = S1.Length-1;
                                s2_cur = S2.Length-1;
                                S1_result[k]="";
                                S2_result[k]="";
                                i_last = 0;
                                j_last = 0;
                                cnt_ins = 0;
                                cnt_del = 0;
                                cnt_rep = 0;
                                both_operation_adjacent_ins = 0;
                                both_operation_adjacent_del = 0;
                                both_operation_adjacent_skip = 0;
                                string result_word;
                                result_word = "";
                                while ((i1 >= 0) & (j1 >= 0))
                                {
                                //  1 "  LEFT   ARROW", 2 "    UP   ARROW", 3 "DiagLU   ARROW", 4 "   DEL  MATRIX", 5 "VARIANT MATRIX"
                                    if (D[i1,j1,5] > 1)
                                    {
                                        i_last = i1;
                                        j_last = j1;
                                    }
                                    if (D[i1,j1,1] == 1) //Insertion    gorizont
                                    {
                                        S1_result[k] = "_" + S1_result[k]; 
                                        S2_result[k] = S2[s2_cur]+S2_result[k]; 
                                        
                                        result_word = S2[s2_cur] + result_word;
                                        both_operation_adjacent_ins++;
                                        both_operation_adjacent_del = 0;
                                        s2_cur--;
                                        j1--;
                                        cnt_ins++;
                                    }
                                else
                                if (D[i1,j1,2] == 1) //Delition   vertical
                                    {
                                    S1_result[k] =  S1[s1_cur] + S1_result[k]; 
                                    S2_result[k] = "_"+S2_result[k]; 
                                    both_operation_adjacent_del++;
                                    both_operation_adjacent_ins =0;
                                    s1_cur--;
                                    i1--;
                                    cnt_del++;
                                }
                                else
                                if (D[i1,j1,3] == 1) 
                                {
                                        if (T[i1,j1] == 1) //Replacement
                                        {
                                            S1_result[k] = S1[s1_cur] + S1_result[k]; 
                                            S2_result[k] = S2[s2_cur]  + S2_result[k]; 

                                            result_word = S2[s2_cur] + result_word;
                                            both_operation_adjacent_del =0;
                                            both_operation_adjacent_ins =0;
                                
                                            s1_cur--;
                                            s2_cur--;
                                            j1--;
                                            i1--;
                                            cnt_rep++;
                                            
                                        }
                                        else //Match
                                        {
                                            S1_result[k] = S1[s1_cur] + S1_result[k]; 
                                            S2_result[k] = S2[s2_cur]  + S2_result[k]; 
                                            result_word = S2[s2_cur] + result_word;
                                            both_operation_adjacent_del =0;
                                            both_operation_adjacent_ins =0;
                                            s1_cur--;
                                            s2_cur--;
                                            j1--;
                                            i1--;
                                            
                                        }
                                }
                                if ((both_operation_adjacent_del == 2) ^ (both_operation_adjacent_ins == 2)) //skip two adjactive operations
                                {
                                    both_operation_adjacent_skip = 1;
                                }
                                if ((i1 == 0) & (j1 == 0))
                                    {
                                        i1=-1;
                                        j1=-1;
                                    }
                            }
                            if  ((((cnt_ins + cnt_del + cnt_rep *2) <=2) & ((cnt_ins + cnt_del + cnt_rep *2) !=0)) &(both_operation_adjacent_skip==0))
                            {
                                compare_found = 1;
                                if ((cnt_ins + cnt_del + cnt_rep * 2) == 1)
                                    {
                                        result_words_1edits.Add(result_word);
                                    }
                                else
                                    {
                                        result_words_2edits.Add(result_word);
                                    }
                                
                            }
                           
                            if ((i_last != 0) & (j_last != 0))
                            {
                                D[i_last,j_last,5] = D[i_last,j_last,5] - 1;
                                if (D[i_last,j_last,1] == 1) //Insertion
                                {
                                    D[i_last,j_last,1] = 0;
                                }
                                else
                                    if (D[i_last,j_last,2] == 1) //Delition
                                    {
                                        D[i_last,j_last,2] = 0;
                                    }
                                    else
                                    if (D[i_last,j_last,3] == 1) //Match or Replacement
                                    {
                                        D[i_last,j_last,3] = 0;
                                    }
                            }
                        }
                    }
                }
                // forming results
                current_word_print++;
                if (Convert.ToInt32(InputPMWords_cnt[0])==current_word_print+47)
                {
                    InputPMWords_cnt =InputPMWords_cnt.Remove(0,1);
                    current_word_print=1;
                    Outf.WriteLine(); 
                }
                if (compare_found==0)   // Not founded in dict
                    {
                        string tmp = @"{"+S1+"?} ";
                        Outf.Write("{0}",tmp);
                    }
                else
                    {
                        if (srt_compare == 0)
                        {
                            if (result_words_1edits.Count > 0)
                            {
                                if (result_words_1edits.Count == 1)
                                {
                                    foreach (string str_ppp_1 in result_words_1edits)
                                    {
                                        string for_print = str_ppp_1 + " ";
                                        Outf.Write("{0}", for_print);
                                    }
                                }
                                else
                                {
                                    string tmp_pp_1 = @"{";
                                    foreach (string str_p_1 in result_words_1edits)
                                    {
                                        tmp_pp_1 = tmp_pp_1 + str_p_1 + " ";
                                    }
                                    tmp_pp_1 = tmp_pp_1.Remove(tmp_pp_1.Length - 1, 1) + "} ";
                                    Outf.Write("{0}", tmp_pp_1);
                                }
                            }
                            else
                            {
                                if (result_words_2edits.Count == 1)
                                {
                                    foreach (string str_ppp in result_words_2edits)
                                    {
                                        string for_print = str_ppp + " ";
                                        Outf.Write("{0}", for_print);
                                    }
                                }
                                else
                                {
                                    string tmp_pp = @"{";
                                    foreach (string str_p in result_words_2edits)
                                    {
                                        tmp_pp = tmp_pp + str_p + " ";
                                    }
                                    tmp_pp = tmp_pp.Remove(tmp_pp.Length - 1, 1) + "} ";
                                    Outf.Write("{0}", tmp_pp);
                                }
                            }
                         }
                        else
                        {
                            Outf.Write("{0}", result_words_0edit + " ");
                        }
                    }
                }
                Outf.Close();
            }
        }
    }
}

