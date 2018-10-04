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
            int strLength = Input.Text.Length;
            string[,] matr = DefineMatrix(strLength, out int matrRow, out int matrCol);
            row = matrRow;
            col = matrCol;
            rowPermutation = new int[row];
            colPermutation = new int[col];
            if (!CheckKeys()) return;
            int index = 0;
            RowKey.Text.Split(' ').ToList().ForEach(_ => rowPermutation[index++] = Convert.ToInt32(_));
            index = 0;
            ColumnKey.Text.Split(' ').ToList().ForEach(_ => colPermutation[index++] = Convert.ToInt32(_));
            Output.Text = Encode(Input.Text, strLength, matr);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int strLength = Input.Text.Length;
            string[,] matr = DefineMatrix(strLength, out int matrRow, out int matrCol);
            row = matrRow;
            col = matrCol;
            rowPermutation = new int[row];
            colPermutation = new int[col];
            if (!CheckKeys()) return;
            int index = 0;
            RowKey.Text.Split(' ').ToList().ForEach(_ => rowPermutation[index++] = Convert.ToInt32(_));
            index = 0;
            ColumnKey.Text.Split(' ').ToList().ForEach(_ => colPermutation[index++] = Convert.ToInt32(_));
            Output.Text = Decode(Input.Text, Input.Text.Length, matr);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Input.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private string[,] DefineMatrix(int strLength, out int matrRow, out int matrCol)
        {
            int col = (int)Math.Sqrt(strLength);
            int row = 2;

            while (col * row < strLength)
            {
                row++;
            }

            matrRow = row;
            matrCol = col;

            return new string[row, col];
        }

        private bool CheckKeys()
        {
            bool result = true;
            var set = new HashSet<int>();
            try
            {
                RowKey.Text.Split(' ').ToList().ForEach(_ => set.Add(Convert.ToInt32(_)));
                result = (set.Count == row);
                set.ToList().ForEach(_ => result = result && 0 < _ && _ <= row);
                set.Clear();
                ColumnKey.Text.Split(' ').ToList().ForEach(_ => set.Add(Convert.ToInt32(_)));
                result = result && (set.Count == col);
                set.ToList().ForEach(_ => result = result && 0 < _ && _ <= col);
            }
            catch (Exception ex)
            {
                result = false;
            }
            if (!result)
                Error.Text = "Ошибка ключа. Проверьте ключ";
            else
                Error.Text = "";
            return result;
        }

        private string Encode(string str, int strLength, string[,] matr)
        {
            string codeStr = "";
            int strIter = 0, r;

            //write
            for (int rp = 0; rp < row; rp++)
            {
                r = rowPermutation[rp] - 1;
                for (int j = 0; j < col; j++)
                {
                    if (strIter < strLength)
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

        private string Decode(string str, int strLength, string[,] matr)
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
