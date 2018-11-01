using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] rowPermutation;
        private int[] colPermutation;
        private int row, col;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckKeys(Input.Text, out row, out col)) return;
            string[,] matr = new string[row, col];
            rowPermutation = new int[row];
            colPermutation = new int[col];
            int index = 0;
            RowKey.Text.Split(' ').ToList().ForEach(_ => rowPermutation[index++] = Convert.ToInt32(_));
            index = 0;
            ColumnKey.Text.Split(' ').ToList().ForEach(_ => colPermutation[index++] = Convert.ToInt32(_));
            Output.Text = Encode(Input.Text, matr);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!CheckKeys(Input.Text, out row, out col)) return;
            string[,] matr = new string[row, col];
            rowPermutation = new int[row];
            colPermutation = new int[col];
            int index = 0;
            RowKey.Text.Split(' ').ToList().ForEach(_ => rowPermutation[index++] = Convert.ToInt32(_));
            index = 0;
            ColumnKey.Text.Split(' ').ToList().ForEach(_ => colPermutation[index++] = Convert.ToInt32(_));
            Output.Text = Decode(Input.Text, matr);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Input.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private bool CheckKeys(string inputString, out int row, out int col)
        {
            bool result = true;
            var set = new HashSet<int>();
            try
            {
                RowKey.Text.Split(' ').ToList().ForEach(_ =>
                {
                    if (set.Contains(Convert.ToInt32(_))) result = false;
                    set.Add(Convert.ToInt32(_));
                });
                int rows = set.Count;
                set.ToList().ForEach(_ => result = result && 0 < _ && _ <= rows);
                set.Clear();
                ColumnKey.Text.Split(' ').ToList().ForEach(_ =>
                {
                    if (set.Contains(Convert.ToInt32(_))) result = false;
                    set.Add(Convert.ToInt32(_));
                });
                int cols = set.Count;
                set.ToList().ForEach(_ => result = result && 0 < _ && _ <= cols);

                row = rows;
                col = cols;

                result = result && (row * cols >= inputString.Length);
            }
            catch (Exception ex)
            {
                row = col = 0;
                result = false;
            }
            if (!result)
            {
                Error.Text = "Ошибка ключа. Проверьте ключ";
                Output.Text = "";
            }
            else
                Error.Text = "";
            return result;
        }

        private string Encode(string str, string[,] matr)
        {
            string codeStr = "";
            int strIter = 0, r;

            //write
            for (int rp = 0; rp < row; rp++)
            {
                r = rowPermutation[rp] - 1;
                for (int j = 0; j < col; j++)
                {
                    if (strIter < str.Length)
                    { //если еще не всю строку переписали
                        matr[r, j] = (str[strIter]).ToString();
                        strIter++;
                    }
                    else
                    {
                        matr[r, j] = " ";
                    }
                }
            }

            //read
            for (int cp = 0; cp < col; cp++)
            {
                for (int i = 0; i < row; i++)
                {
                    codeStr += matr[i, colPermutation[cp] - 1];
                }
            }

            return codeStr;
        }

        private string Decode(string str, string[,] matr)
        {
            string decodeStr = "";
            int strIter = 0, c;


            //write
            for (int cp = 0; cp < col; cp++)
            {
                c = colPermutation[cp] - 1;
                for (int i = 0; i < row; i++)
                {
                    matr[i, c] = (str[strIter]).ToString();
                    strIter++;
                }
            }

            //read
            for (int rp = 0; rp < row; rp++)
            {
                for (int j = 0; j < col; j++)
                    decodeStr += matr[rowPermutation[rp] - 1, j];
            }

            return decodeStr;
        }

    }
}
